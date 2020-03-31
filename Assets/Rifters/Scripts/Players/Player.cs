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
	public bool isOfflinePlayer;
	[Header("Player components")]
	public Transform playerOrigin;

	public CinemachineFreeLook cameraRig;
	public Camera viewCamera;
	public AudioListener audioListener;

	public PlayerPhysicsWalker playerPhysicsWalker;

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

	void Update()
	{
		// Rotating the camera
		float mouseX = Input.GetAxis("Mouse X") * mouseAcceleration * Time.fixedDeltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseAcceleration * Time.fixedDeltaTime;
		cameraRig.m_XAxis.Value += mouseX;
		cameraRig.m_YAxis.Value -= mouseY / 180f;
	}

	private void LateUpdate()
	{
		// Updating player origin
		playerOrigin.position = playerPhysicsWalker.transform.position;
		playerOrigin.rotation = Quaternion.Euler(0, viewCamera.transform.rotation.eulerAngles.y, 0);
	}
}