using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class Player : NetworkBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public bool isOfflinePlayer;
	public bool isPlayer1;
	[Header("Player components")]
	public CinemachineFreeLook cameraRig;
	public RigidbodyController rigidbodyController;
	public Animator animator;
	public AnimationState state;

	[Header("Spell components")]
	public ExploderSpell exploderSpell;
	public WallSpell wallSpell;
	public BlinkSpell blinkSpell;

	[Header("Camera settings")]
	public float mouseAcceleration = 100f;
	public float joystickAcceleration = 120f;
	#endregion

	//
	// Private variables
	#region Private variables
	private string cameraX;
	private string cameraY;
	private float acceleration;

	public string horizontalKey = "Horizontal";
	public string verticalKey = "Vertical";
	public string jumpKey = "Jump";
	private string aimKey = "Fire2";
	private string triggerKey = "Fire1";
	private string blinkKey = "Fire3";

	private float nextControlTime = 0f;
	#endregion

	//--------------------------
	// NetworkBehaviour events
	//--------------------------
	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		cameraRig.gameObject.SetActive(true);
	}

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void Awake()
	{
		#region Check Player 1 or 2
		//-- We don't need to check anymore if it's player one or two --
		//-- Will leave it here if we wan't to make local multi some day --
		/*
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
		}*/
		#endregion

		// TODO: multiplayer input thingy?
		horizontalKey = "Horizontal P1";
		verticalKey = "Vertical P1";
		jumpKey = "Jump P1";

		cameraX = "P1 Camera X";
		cameraY = "P1 Camera Y";
		acceleration = mouseAcceleration;

		aimKey = "Fire2 P1";
		triggerKey = "Fire1 P1";
		blinkKey = "Fire3 P1";
	}

	private void Update()
	{
		// Multiplayer check
		if (!hasAuthority) return;

		// Control freeze check
		if (Time.time < nextControlTime) return;

		// Spells
		wallSpell.isAiming = Input.GetButton(aimKey);
		if (wallSpell.isAiming)
		{
			if (Input.GetButtonDown(triggerKey)) wallSpell.Trigger();
		}
		else if(Input.GetButtonDown(triggerKey))
			exploderSpell.Trigger(); // exploder

		if (Input.GetButtonDown(blinkKey))
		{
			blinkSpell.Trigger();
		}

		// Camera rotation
		float mouseX = Input.GetAxisRaw(cameraX) * acceleration * Time.fixedDeltaTime;
		float mouseY = Input.GetAxisRaw(cameraY) * acceleration * Time.fixedDeltaTime;

		cameraRig.m_XAxis.Value += mouseX;
		cameraRig.m_YAxis.Value -= mouseY / 180f;

		// RigidbodyController
		Vector3 newRotation = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		rigidbodyController.transform.rotation = Quaternion.Euler(newRotation);
		rigidbodyController.axisV = Input.GetAxisRaw(verticalKey);
		rigidbodyController.axisH = Input.GetAxisRaw(horizontalKey);
		if (Input.GetButtonDown(jumpKey)) rigidbodyController.Jump();

		// Animator
		Vector3 velocity = rigidbodyController.rigidbody.velocity;
		Vector3 localVelocity = rigidbodyController.transform.InverseTransformDirection(velocity);

		animator.SetBool("IsGrounded", rigidbodyController.isGrounded);
		animator.SetFloat("Forward", localVelocity.z / rigidbodyController.maxSprintingSpeed);
		animator.SetFloat("Sideways", localVelocity.x / rigidbodyController.maxSprintingSpeed);
		animator.SetBool("IsAccelerating", Mathf.Abs(Input.GetAxisRaw(verticalKey) + Mathf.Abs(Input.GetAxisRaw(horizontalKey))) > 0);
	}

	public void SetAnimBool(string valueString, bool valueBool)
	{
		animator.SetBool(valueString, valueBool);
	}
	public void SetAnimTriggerSpell(string valueString, float time)
	{
		animator.SetTrigger(valueString);
	}

	//--------------------------
	// Player events
	//--------------------------
	public void FreezeControls(float duration)
	{
		float nextTime = Time.time + duration;
		if (nextTime > nextControlTime) nextControlTime = Time.time + duration;

		rigidbodyController.axisV = 0f;
		rigidbodyController.axisH = 0f;
	}

	public void UnfreezeControls()
	{
		nextControlTime = 0f;
	}
}