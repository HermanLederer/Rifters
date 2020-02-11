using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
	// Other components
	private CharacterController _characterController;

	// Editor variables
	public PlayerMovement playerMovement;

	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		_characterController = GetComponent<CharacterController>();
	}

	void Start()
	{
		PhotonNetwork.NickName = Time.fixedTime.ToString();
	}

	void Update()
	{
		
	}
}