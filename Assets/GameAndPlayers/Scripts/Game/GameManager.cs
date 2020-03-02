using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
	// Editor variables
	[SerializeField] private bool offlineMode = false;
	[SerializeField] private GameObject playerPrefab = null;
	[SerializeField] private GameObject gameBallPrefab = null;
	[SerializeField] private int playToGoals = 5;

	// Public variables
	public int team1score = 0;
	public int team2score = 0;

	// Private variables
	private bool isGamePlaying;


	// Static variables
	public static GameManager instance;

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	private void Awake()
	{
		isGamePlaying = true;
		
		// singleton
		if (instance != null)
			Destroy(gameObject);

		instance = this;
	}

	void Start()
    {
		if (!offlineMode) // online mode
		{
			// instantiating local player
			PhotonNetwork.Instantiate(playerPrefab.name, transform.position + Vector3.one * Random.Range(-2, 2), transform.rotation);

			// instantiating online game objects
			if (PhotonNetwork.LocalPlayer.IsMasterClient)
			{
				PhotonNetwork.Instantiate(gameBallPrefab.name, transform.position, transform.rotation);
			}
		}
		else
		{
			GetComponent<PhotonView>().enabled = false;
			Instantiate(gameBallPrefab, transform.position, transform.rotation);
		}
    }

	void FixedUpdate()
	{
		// winning conditions
		if (isGamePlaying)
		{
			if (team1score >= playToGoals || team2score >= playToGoals)
			{
				if (team1score > team2score)
					DeclareWiner(GameTeam.Team1);
				else
					DeclareWiner(GameTeam.Team2);
			}
		}
		else
		{
			if (Input.GetButton("Cancel"))
			{
				Time.timeScale = 1f;
				if (!offlineMode)
					PhotonNetwork.LoadLevel("Game");
				else
					SceneManager.LoadScene("Game");
			}
		}
	}

	//--------------------------
	// MonoBehaviourPunCallbacks events
	//--------------------------
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		Debug.Log("Opponent left the room");
		//PhotonNetwork.LeaveRoom();
		//PhotonNetwork.LoadLevel("Menu");
	}

	//--------------------------
	// IPunObservable methods
	//--------------------------
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting) // stream is writing
		{
			stream.SendNext(team1score);
			stream.SendNext(team2score);
		}
		else // stream is reading
		{
			team1score = (int)stream.ReceiveNext();
			team2score = (int)stream.ReceiveNext();
		}
	}

	//--------------------------
	// GameManager methods
	//--------------------------
	public void Score(GameTeam team)
	{
		// increasing the score
		if (team == GameTeam.Team1) ++team1score;
		else ++team2score;

		// debugging
		Debug.Log("A goal has been scored by " + team);
		Debug.Log("" + GameTeam.Team1 + ": " + team1score + "; " + GameTeam.Team2 + ": " + team2score + ";");
	}

	public void DeclareWiner(GameTeam team)
	{
		Debug.Log(team + " won!");
		isGamePlaying = false;
		Time.timeScale = 0.1f;
	}
}
