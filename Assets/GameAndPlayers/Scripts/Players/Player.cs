using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
	//
	// Editor Variables
	#region Editor variables
	[Header("Player components")]
	public CinemachineFreeLook cameraRig;
	public Camera viewCamera;
	public AudioListener audioListener;
	public PlayerCharacterModel characterModel;
	public Transform playerOrigin;
	public Transform orientationTransform;

	[Header("Camera settings")]
	public float mouseAcceleration = 100f;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		//if (photonView.IsMine) audioListener.enabled = true;
		//if (photonView.IsMine) viewCamera.enabled = true;
		//if (photonView.IsMine) viewCamera.enabled = true;
	}

	void Start()
	{
		//if (isOfflinePlayer) GameManager.instance.players.Add(GetComponent<Player>());
		//PhotonNetwork.NickName = Time.fixedTime.ToString();
	}

	void Update()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseAcceleration * Time.fixedDeltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseAcceleration * Time.fixedDeltaTime;

		// rotating the camera
		cameraRig.m_XAxis.Value += mouseX;
		cameraRig.m_YAxis.Value -= mouseY / 180f;
	}
}