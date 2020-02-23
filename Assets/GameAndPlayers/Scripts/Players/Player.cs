using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
	// Editor variables
	public PlayerMovement playerMovement;
	public CharacterController characterController;
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
		if (photonView.IsMine) audioListener.enabled = true;
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