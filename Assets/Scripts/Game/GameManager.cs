using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	// Editor variables
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
		// instantiating players
		PhotonNetwork.Instantiate(playerPrefab.name, transform.position + Vector3.one * Random.Range(-3, 3), transform.rotation);
    }

	//--------------------------
	// GameManager methods
	//--------------------------
	public void Score(GameTeam team)
	{
		
	}
}
