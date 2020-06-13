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

	public List<GameObject> spawnableObjects = null;

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

	private bool altControls = false;
	private string horizontalKey = "Horizontal";
	private string verticalKey = "Vertical";
	private string jumpKey = "Jump";
	private string aimKey = "Fire2";
	private string triggerKey = "Fire1";
	private string blinkKey = "Fire3";

	private float nextControlTime = 0f;

	private NetworkGamePlayerRifters myPlayer;
	#endregion

	//--------------------------
	// NetworkBehaviour events
	//--------------------------
	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		cameraRig.gameObject.SetActive(true);

		NetworkManagerRifter room = NetworkManager.singleton as NetworkManagerRifter;

		foreach (var player in room.GamePlayers)
		{
			if (player.hasAuthority)
			{
				myPlayer = player;
				myPlayer.myPlayer = this;
				break;
			}
		}

		if(myPlayer == null)
		{
			Debug.LogError("I couldn't find your Game Player");
		}
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

		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Start()
	{
		// Disabeling components that are not needed on "the other player"
		if (!hasAuthority)
		{
			rigidbodyController.Rb.isKinematic = true;
			rigidbodyController.enabled = false;
		}
	}

	private void Update()
	{
		// Multiplayer check
		if (!hasAuthority) return;

		// If the player is on the pause menu
		if (myPlayer && !myPlayer.isPaused)
		{
			// Control freeze check
			if (Time.time < nextControlTime) return;

			// Spells
			if (Input.GetButtonDown("Alt controls")) altControls = !altControls;

			if (altControls)
			{
				// Alternative controls
				if (Input.GetButtonDown(aimKey))
					wallSpell.isAiming = true;

				if (Input.GetButtonUp(aimKey))
				{
					wallSpell.isAiming = false;
					wallSpell.Trigger();
				}

				if (Input.GetButtonDown(triggerKey))
					exploderSpell.Trigger(); // exploder
			}
			else
			{
				// Normal controls
				wallSpell.isAiming = Input.GetButton(aimKey);
				if (wallSpell.isAiming)
				{
					if (Input.GetButtonDown(triggerKey)) wallSpell.Trigger();
				}
				else if (Input.GetButtonDown(triggerKey))
					exploderSpell.Trigger(); // exploder
			}

			if (Input.GetButtonDown(blinkKey))
			{
				blinkSpell.direction = new Vector2(Input.GetAxisRaw(horizontalKey), Input.GetAxisRaw(verticalKey));
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
		}

		// Animator
		Vector3 velocity = rigidbodyController.Rb.velocity;
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
	public void SetAnimTriggerSpell(string valueString)
	{
		animator.SetTrigger(valueString);
	}

	[Command]
	public void CmdSpawnObject(int spawnablePrefabId, float px, float py, float pz, float rx, float ry, float rz, float rw)
	{
		GameObject obj = Instantiate(spawnableObjects[spawnablePrefabId], new Vector3(px, py, pz), new Quaternion(rx, ry, rz, rw));
		NetworkServer.Spawn(obj);
	}

	[Command]
	public void CmdSpawnChildObject(int spawnablePrefabId, float px, float py, float pz, float rx, float ry, float rz, float rw)
	{
		GameObject obj = Instantiate(spawnableObjects[spawnablePrefabId], rigidbodyController.transform);
		obj.transform.localPosition = new Vector3(px, py, pz);
		obj.transform.localRotation = new Quaternion(rx, ry, rz, rw);
		NetworkServer.Spawn(obj);
	}

	//--------------------------
	// Player methods
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

	public void ResetPlayerPosition()
	{
		rigidbodyController.transform.position = transform.position;
		rigidbodyController.transform.rotation = transform.rotation;
	}

	public void ChangeSpellAlpha(TypeOfSpell spell, float alphaValue)
	{
		myPlayer.ChangeSpellAlpha(spell, alphaValue);
	}
}