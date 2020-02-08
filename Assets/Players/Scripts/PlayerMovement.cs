using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	// Other components
	public CharacterController _characterController { get; private set; }

	// Editor Variables
	[SerializeField]
	private Transform cameraTransform;
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
		_characterController = GetComponent<CharacterController>();
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
		// Camera rotation
		float mouseX = Input.GetAxis("Mouse X") * mouseAcceleration * Time.deltaTime;
		float mouseY =  Input.GetAxis("Mouse Y") * mouseAcceleration * Time.deltaTime;

		cameraTransform.localRotation = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x - mouseY, 0f, 0f);
		transform.Rotate(Vector3.up * mouseX);
	}

	private void FixedUpdate()
	{
		// Smooth locomotion
		Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");
		Vector3 sidewaysMovement = transform.right * Input.GetAxis("Horizontal");

		Vector3 direction = forwardMovement + sidewaysMovement;
		//direction = Quaternion.Euler(0, headTransform.rotation.eulerAngles.y, 0) * direction;

		// Gravity
		Vector3 gravity = Physics.gravity * 60 * Time.fixedDeltaTime;

		// combined movement
		_characterController.Move((Vector3.ClampMagnitude(direction, 1.0f) * movementSpeed + gravity) * Time.fixedDeltaTime);

		// Sticking to slopes
		if ((direction.x != 0 || direction.z != 0) && IsOnSlope())
			_characterController.Move(Vector3.down * _characterController.height / 2 * 8 * Time.fixedDeltaTime);
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