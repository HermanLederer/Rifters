using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviourPun
{
	// Editor variables
	public bool isOfflinePlayer = false;
	public PlayerMovement playerMovement;
	public GameObject head;
	public Camera viewCamera;
	public AudioListener audioListener;
	
	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		if (isOfflinePlayer) GameManager.instance.players.Add(GetComponent<Player>());

		if (photonView.IsMine) audioListener.enabled = true;
		if (photonView.IsMine) viewCamera.enabled = true;
		if (photonView.IsMine) viewCamera.enabled = true;
	}

	void Start()
	{
		PhotonNetwork.NickName = Time.fixedTime.ToString();
	}

	void Update()
	{
		
	}
}