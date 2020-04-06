using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

[RequireComponent(typeof(InGameUI))]
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
	// Singleton
	public static GameManager instance;

	// Other components
	private InGameUI inGameUI;

	// Editor variables
	[SerializeField] private bool offlineMode = false;
	[SerializeField] private bool respawnPlayersAfterGoal = true;
	[SerializeField] private GameObject playerPrefab = null;
	[SerializeField] private GameObject gameBallPrefab = null;
	[SerializeField] private int playToGoals = 5;

	[SerializeField] private AudioClip Goal;

	// Public variables
	[HideInInspector] public int team1score = 0;
	[HideInInspector] public int team2score = 0;
	[HideInInspector] public GameTeam localPlayerTeam = GameTeam.Team1;
	[HideInInspector] public List<Player> players;



	// Private variables
	private bool isGamePlaying;

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	private void Awake()
	{
		// singleton
		if (instance != null)
			Destroy(gameObject);
		else
			instance = this;

		inGameUI = GetComponent<InGameUI>();

		isGamePlaying = true;
		players = new List<Player>();
	}

	void Start()
    {
		if (!offlineMode) // online mode
		{
			// instantiating local player
			players.Add(PhotonNetwork.Instantiate(playerPrefab.name, transform.position + Vector3.one * Random.Range(-2, 2), transform.rotation).GetComponent<Player>());

			// instantiating online game objects
			if (PhotonNetwork.LocalPlayer.IsMasterClient)
			{
				PhotonNetwork.Instantiate(gameBallPrefab.name, transform.position, transform.rotation);
			}
		}
		else
		{
			GetComponent<PhotonView>().enabled = false;
			Instantiate(gameBallPrefab, Vector3.zero, Quaternion.identity);
		}

		localPlayerTeam = GameTeam.Team1;

		if (localPlayerTeam == GameTeam.Team1)
			inGameUI.yourTeam1.gameObject.SetActive(true);
		else
			inGameUI.yourTeam2.gameObject.SetActive(true);
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

		inGameUI.score1.text = "" + team1score;
		inGameUI.score2.text = "" + team2score;

		// debugging
		Debug.Log("A goal has been scored by " + team);
		Debug.Log("" + GameTeam.Team1 + ": " + team1score + "; " + GameTeam.Team2 + ": " + team2score + ";");
		respawnPlayersAndBall();
	}

	public void DeclareWiner(GameTeam team)
	{
		Debug.Log(team + " won!");
		isGamePlaying = false;
		Time.timeScale = 0.1f;
	}

	public void Win()
	{
		Debug.Log("The player finished the game");
		inGameUI.LoadWinningScreen();
		isGamePlaying = false;
		//Time.timeScale = 0.1f;
	}
	public void respawnPlayersAndBall()
	{
		AudioManager.Instance.PlayJump(Goal);
		GameObject.FindGameObjectWithTag("Dragon").GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		GameObject.FindGameObjectWithTag("Dragon").transform.position = GameObject.Find("BallSpawnPosition").transform.position;
		if (respawnPlayersAfterGoal)
		{
			// going to respawn players based on their team number later
			GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("Team1SpawnPosition").transform.position;
		}
	}
}
