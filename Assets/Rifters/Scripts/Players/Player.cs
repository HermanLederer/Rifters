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

	public Animator animator;
	public AnimationState state;

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

		// Updating the animator
		Vector3 velocity = playerPhysicsWalker.rigidbody.velocity;
		Vector3 localVelocity = playerOrigin.InverseTransformDirection(velocity);

		Vector2 horizontalVelocity = new Vector2(localVelocity.x, localVelocity.z);
		float verticalVelocity = playerPhysicsWalker.rigidbody.velocity.y;

		animator.SetBool("IsGrounded", playerPhysicsWalker.isGrounded);		
		animator.SetFloat("Forward", localVelocity.z / playerPhysicsWalker.maxSprintingSpeed);
		animator.SetFloat("Sideways", localVelocity.x / playerPhysicsWalker.maxSprintingSpeed);
		animator.SetFloat("Upward", verticalVelocity);
		animator.SetFloat("Horizontal", horizontalVelocity.magnitude / playerPhysicsWalker.maxSprintingSpeed);
		animator.SetBool("IsAccelerating", !(Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0));
	}

	private void OnDrawGizmos()
	{
		//Vector3 center = playerOrigin.position + Vector3.up;

		//if (animator.GetBool("IsAccelerating")) Gizmos.DrawSphere(center, 1f);
	}
}