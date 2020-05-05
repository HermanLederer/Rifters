using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
	//
	// Editor variables
	#region Editor variables
	public bool isOfflinePlayer;
	public bool isPlayer1;
	[Header("Player components")]
	public CinemachineFreeLook cameraRig;
	public Camera viewCamera;
	public AudioListener audioListener;

	public RigidbodyController rigidbodyController;

	public Animator animator;
	public AnimationState state;

	[Header("Input")]
	public string horizontalKey = "Horizontal";
	public string verticalKey = "Vertical";
	public string jumpKey = "Jump";

	[Header("Camera settings")]
	public float mouseAcceleration = 100f;
	public float joystickAcceleration = 120f;
	#endregion

	//
	// Private variables
	private string cameraX;
	private string cameraY;
	private float acceleration;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void Awake()
	{
		if (isPlayer1)
		{
			horizontalKey = "Horizontal P1";
			verticalKey = "Vertical P1";
			jumpKey = "Jump P1";
		}
		else
		{
			horizontalKey = "Horizontal P2";
			verticalKey = "Vertical P2";
			jumpKey = "Jump P2";
		}

		if (isPlayer1)
		{
			cameraX = "P1 Camera X";
			cameraY = "P1 Camera Y";
			acceleration = mouseAcceleration;
		}
		else
		{
			cameraX = "P2 Camera X";
			cameraY = "P2 Camera Y";
			acceleration = joystickAcceleration;
		}
	}

	void FixedUpdate()
	{
		float mouseX = Input.GetAxis(cameraX) * acceleration * Time.fixedDeltaTime;
		float mouseY = Input.GetAxis(cameraY) * acceleration * Time.fixedDeltaTime;
		
		cameraRig.m_XAxis.Value += mouseX;
		cameraRig.m_YAxis.Value -= mouseY / 180f;

		// Using input in rigidbody cntroller
		rigidbodyController.axisV = Input.GetAxisRaw(verticalKey);
		rigidbodyController.axisH = Input.GetAxisRaw(horizontalKey);
		rigidbodyController.transform.rotation = Quaternion.Euler(0, viewCamera.transform.rotation.eulerAngles.y, 0);
		if (Input.GetButton(jumpKey)) rigidbodyController.Jump();
	}

	private void LateUpdate()
	{
		// Updating the animator
		Vector3 velocity = rigidbodyController.rigidbody.velocity;
		Vector3 localVelocity = rigidbodyController.transform.InverseTransformDirection(velocity);

		animator.SetBool("IsGrounded", rigidbodyController.isGrounded);		
		animator.SetFloat("Forward", localVelocity.z / rigidbodyController.maxSprintingSpeed);
		animator.SetFloat("Sideways", localVelocity.x / rigidbodyController.maxSprintingSpeed);
		animator.SetBool("IsAccelerating", !((Input.GetAxisRaw(verticalKey) + Input.GetAxisRaw(horizontalKey)) == 0));
	}

	public void SetAnimBool(string valueString, bool valueBool)
	{
		animator.SetBool(valueString, valueBool);
	}
	public void SetAnimTriggerSpell(string valueString, float time)
	{
		animator.SetTrigger(valueString);
		//playerPhysicsWalker.SetSpellTime(time); // we don't need to freeze the player
	}

	private void OnDrawGizmos()
	{
		//Vector3 center = playerOrigin.position + Vector3.up;

		//if (animator.GetBool("IsAccelerating")) Gizmos.DrawSphere(center, 1f);
	}
}