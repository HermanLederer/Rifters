using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
	// Other components
	public Player _player { get; private set; }
	public CharacterController _characterController { get; private set; }
	public PhotonView _photonView { get; private set; }

	// Editor Variables
	public bool isOfflinePlayer = false;
	public float walkingPower = 4f;
	public float sprintingPower = 7f;
	public float walkJumpingPower = 7f;
	public float sprintJumpingPower = 7f;
	public float mouseAcceleration = 100f;

	// Public variables
	[HideInInspector]
	public Vector3 spawnPoint;

	// Private variables
	public Vector3 velocity { get; private set; }

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		_player = GetComponent<Player>();
		_characterController = _player.characterController;
		_photonView = _player.photonView;
	}

	private void Start()
	{
		// Public variables
		spawnPoint = transform.position;

		// Private variables
		velocity = Vector3.zero;

		Cursor.lockState = CursorLockMode.Locked;
	}

	private void FixedUpdate()
	{
		if (_photonView.IsMine || isOfflinePlayer)
		{
			// Camera rotation
			float mouseX = Input.GetAxis("Mouse X") * mouseAcceleration * Time.fixedDeltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * mouseAcceleration * Time.fixedDeltaTime;
			_player.head.transform.localRotation = Quaternion.Euler(_player.head.transform.localRotation.eulerAngles.x - mouseY, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);

			// Resetting velocities
			if (_characterController.isGrounded)
				velocity = Vector3.zero;

			// Gravity
			velocity += Physics.gravity * Time.fixedDeltaTime;

			// Smooth locomotion
			Vector3 forwardMovement = transform.forward * Input.GetAxisRaw("Vertical");
			Vector3 sidewaysMovement = transform.right * Input.GetAxisRaw("Horizontal");
			if (_characterController.isGrounded)
			{
				if (Input.GetButton("Sprint"))
					velocity += (forwardMovement + sidewaysMovement).normalized * sprintingPower;
				else
					velocity += (forwardMovement + sidewaysMovement).normalized * walkingPower;
			}

			// Jumping
			if (Input.GetButton("Jump") && _characterController.isGrounded)
			{
				if (Input.GetButton("Sprint"))
					velocity += new Vector3(0, sprintJumpingPower, 0);
				else
					velocity += new Vector3(0, walkJumpingPower, 0);
			}

			// Rotating towards camera
			//direction = Quaternion.Euler(0, headTransform.rotation.eulerAngles.y, 0) * direction;

			// combined movement
			_characterController.Move(velocity * Time.fixedDeltaTime);

			// Sticking to slopes
			if ((velocity.x != 0 || velocity.z != 0))
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position, Vector3.down, out hit, _characterController.height / 2 + 0.0001f))
					velocity += Vector3.down * 0.2f; // slope magnet force
			}
		}
	}

	//--------------------------
	// PlayerMovement methods
	//--------------------------
}