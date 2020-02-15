using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManagerData : MonoBehaviour, IPunObservable
{
	// GameManager variables
	public int team1score;
	public int team2score;

	// Static variables
	public static GameManagerData instance;

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

	//--------------------------
	// IPunObservable methods
	//--------------------------
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		throw new System.NotImplementedException();
	}
}
