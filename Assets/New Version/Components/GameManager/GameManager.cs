using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InGameUI))]
public class GameManager : MonoBehaviour
{
	// Singleton
	public static GameManager instance;

	// Other components
	private InGameUI inGameUI;

	// Editor variables
	//[SerializeField] private bool offlineMode = false;
	[SerializeField] private bool respawnPlayersAfterGoal = true;
	[SerializeField] private int playToGoals = 5;
	
	[Header("Audio")]
	//[SerializeField] private AudioClip goal = null;
	[SerializeField] private AudioClip goalDrums = null;
	[SerializeField] private AudioClip goalVoc = null;

	// Public variables
	[HideInInspector] public int team1score = 0;
	[HideInInspector] public int team2score = 0;
	[HideInInspector] public GameTeam localPlayerTeam = GameTeam.Team1;
	[HideInInspector] public List<Player> players;

	// Private variables
	private bool isGamePlaying;
	private GameObject gameBall;

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
		localPlayerTeam = GameTeam.Team1;

		if (localPlayerTeam == GameTeam.Team1)
			inGameUI.yourTeam1.gameObject.SetActive(true);
		else
			inGameUI.yourTeam2.gameObject.SetActive(true);

		gameBall = GameObject.FindGameObjectWithTag("Dragon");
	}

	void Update()
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
				SceneManager.LoadScene("Game");
			}
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

		//Debug.Log("A goal has been scored by " + team);
		//Debug.Log("" + GameTeam.Team1 + ": " + team1score + "; " + GameTeam.Team2 + ": " + team2score + ";");

		AudioManager.instance.PlayDrum(goalDrums);
		AudioManager.instance.PlayTribeVoc(goalVoc);

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
		//AudioManager.instance.PlayJump(Goal);
		//gameBall.transform.position = ballSpawn.position;
		gameBall.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		
		if (respawnPlayersAfterGoal)
		{
			// going to respawn players based on their team number later
			/*GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject p in Players)
			{
				Transform spawnPos = GameObject.Find("Team1SpawnPosition").transform;
				p.transform.position = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z);
				spawnPos.position.x += 10;
			}*/
			//GameObject.Find("Player").transform.position = GameObject.Find("Team1SpawnPosition").transform.position;
			//GameObject.Find("Player 2").transform.position = GameObject.Find("Team2SpawnPosition").transform.position;
		}
	}
}
