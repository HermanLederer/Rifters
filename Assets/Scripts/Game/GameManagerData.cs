using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManagerData : MonoBehaviour, IPunObservable
{
	// GameManager variables
	// (not using a dictuionary to minimize network load)
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
}
