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
	public float movementSpeed = 7f;
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

	private void Update()
	{
		if (_photonView.IsMine)
		{
			// Camera rotation
			float mouseX = Input.GetAxis("Mouse X") * mouseAcceleration * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * mouseAcceleration * Time.deltaTime;

			_player.head.transform.localRotation = Quaternion.Euler(_player.head.transform.localRotation.eulerAngles.x - mouseY, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);

			// Smooth locomotion
			Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");
			Vector3 sidewaysMovement = transform.right * Input.GetAxis("Horizontal");

			Vector3 direction = forwardMovement + sidewaysMovement;
			//direction = Quaternion.Euler(0, headTransform.rotation.eulerAngles.y, 0) * direction;

			// combined movement
			_characterController.SimpleMove(Vector3.ClampMagnitude(direction, 1.0f) * movementSpeed);

			// Sticking to slopes
			if ((direction.x != 0 || direction.z != 0) && IsOnSlope())
				_characterController.Move(Vector3.down * _characterController.height / 2 * 8 * Time.deltaTime);
		}
	}

	//--------------------------
	// PlayerMovement methods
	//--------------------------
	public bool IsOnSlope()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit, _characterController.height / 2 + 1))
			if (hit.normal != Vector3.up)
				return true;

		return false;
	}
}