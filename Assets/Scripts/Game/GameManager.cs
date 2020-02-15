using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	// Editor variables
	[SerializeField] private bool offlineMode;
	[SerializeField] private GameObject gameManagerDataPrefab;
	[SerializeField] private GameObject playerPrefab;

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
			// instantiating players
			PhotonNetwork.Instantiate(playerPrefab.name, transform.position + Vector3.one * Random.Range(-3, 3), transform.rotation);
			PhotonNetwork.Instantiate(gameManagerDataPrefab.name, transform.position + Vector3.one * Random.Range(-3, 3), transform.rotation);
		}
		else
		{
			GameManagerData data = Instantiate(gameManagerDataPrefab, transform.position, transform.rotation).GetComponent<GameManagerData>();
			data.GetComponent<PhotonView>().enabled = false;
		}
    }

	//--------------------------
	// GameManager methods
	//--------------------------
	public void Score(GameTeam team)
	{
		Debug.Log("A goal has been scored by" + team);
	}
}
