using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	// Editor variables
	[SerializeField] private bool offlineMode;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject gameManagerDataPrefab;
	[SerializeField] private GameObject gameBallPrefab;

	// Private variables


	// Static variables
	public static GameManager instance;

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	private void Awake()
	{
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
			PhotonNetwork.Instantiate(playerPrefab.name, transform.position + Vector3.one * Random.Range(-3, 3), transform.rotation);

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
}
