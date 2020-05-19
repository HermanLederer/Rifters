using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player1 : MonoBehaviour
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

	[Header("Spell components")]
	public ExploderSpell exploderSpell;
	public BlinkController blinkSpell;
	public AimIceWall iceWallSpell;

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
	private string chargeKey = "Fire2";
	private string fireKey = "Fire1";
	#endregion

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

		// TODO: multiplayer input thingy?
		chargeKey = "Fire2 P1";
		fireKey = "Fire1 P1";
	}

	private void Update()
	{
		// Mouse clicks
		if (Input.GetButtonDown(chargeKey))
		{
			exploderSpell.Charge();
		}

		if (Input.GetButtonDown(fireKey))
		{
			if (Input.GetButton(chargeKey)) exploderSpell.Shoot();
			//else
			// engage the wall spell here

		}

		// Blink Management
		if (Input.GetKeyDown(KeyCode.LeftShift) && blinkSpell.CheckEnergy())
		{
			blinkSpell.Blink();
		}

		// Ice wall management
		if (iceWallSpell.CheckCooldown())
		{
			if (!iceWallSpell.IsAiming())
			{
				if (Input.GetKeyDown(KeyCode.LeftControl))
				{
					iceWallSpell.SetAiming(true);
				}
			}
			else
			{
				if (iceWallSpell.Aim())
				{
					if (Input.GetKeyUp(KeyCode.LeftControl))
					{
						iceWallSpell.PlaceWall();
					}
				}
				else
				{
					if (Input.GetKeyUp(KeyCode.LeftControl))
					{
						iceWallSpell.SetAiming(false);
					}
				}
			}
		}


		float mouseX = Input.GetAxis(cameraX) * acceleration * Time.fixedDeltaTime;
		float mouseY = Input.GetAxis(cameraY) * acceleration * Time.fixedDeltaTime;

		cameraRig.m_XAxis.Value += mouseX;
		cameraRig.m_YAxis.Value -= mouseY / 180f;

		// Controlling the RigidbodyController
		rigidbodyController.axisV = Input.GetAxisRaw(verticalKey);
		rigidbodyController.axisH = Input.GetAxisRaw(horizontalKey);
		rigidbodyController.transform.rotation = Quaternion.Euler(0, viewCamera.transform.rotation.eulerAngles.y, 0);
		if (Input.GetButtonDown(jumpKey)) rigidbodyController.Jump();

		// Updating the animator
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
		//playerPhysicsWalker.SetSpellTime(time); // we don't need to freeze the player
	}

	private void OnDrawGizmos()
	{
		//Vector3 center = playerOrigin.position + Vector3.up;

		//if (animator.GetBool("IsAccelerating")) Gizmos.DrawSphere(center, 1f);
	}
}