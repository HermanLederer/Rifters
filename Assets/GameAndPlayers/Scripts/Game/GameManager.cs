using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
	// Editor variables
	[SerializeField] private bool offlineMode = false;
	[SerializeField] private GameObject playerPrefab = null;
	[SerializeField] private GameObject gameManagerDataPrefab = null;
	[SerializeField] private GameObject gameBallPrefab = null;
	[SerializeField] private int playToGoals = 5;

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
				PhotonNetwork.Instantiate(gameManagerDataPrefab.name, transform.position, transform.rotation);
				PhotonNetwork.Instantiate(gameBallPrefab.name, transform.position, transform.rotation);
			}
		}
		else
		{
			GameManagerData data = Instantiate(gameManagerDataPrefab, transform.position, transform.rotation).GetComponent<GameManagerData>();
			data.GetComponent<PhotonView>().enabled = false;
			Instantiate(gameBallPrefab, transform.position, transform.rotation);
		}
    }

	void FixedUpdate()
	{
		// winning conditions
		if (isGamePlaying)
		{
			if (GameManagerData.instance.team1score >= playToGoals || GameManagerData.instance.team2score >= playToGoals)
			{
				if (GameManagerData.instance.team1score > GameManagerData.instance.team2score)
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
	// GameManager methods
	//--------------------------
	public void Score(GameTeam team)
	{
		// increasing the score
		if (team == GameTeam.Team1) ++GameManagerData.instance.team1score;
		else ++GameManagerData.instance.team2score;

		// debugging
		Debug.Log("A goal has been scored by " + team);
		Debug.Log("" + GameTeam.Team1 + ": " + GameManagerData.instance.team1score + "; " + GameTeam.Team2 + ": " + GameManagerData.instance.team2score + ";");
	}

	public void DeclareWiner(GameTeam team)
	{
		Debug.Log(team + " won!");
		isGamePlaying = false;
		Time.timeScale = 0.1f;
	}
}
