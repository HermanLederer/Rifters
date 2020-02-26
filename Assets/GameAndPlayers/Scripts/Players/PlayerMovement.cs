using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviourPun
{
	// Other components
	public Player player { get; private set; }

	// Editor Variables
	public bool isOfflinePlayer = false;
	public float walkingPower = 4f;
	public float sprintingPower = 7f;
	public float maxSpeed = 7f;
	public float maxAcceleration = 1f;
	public float maxDeceleration = 2f;
	public float jumpCooldown = 1f;
	public float mouseAcceleration = 100f;
	public GameObject jumpingPlatfiormPrafab = null;
	public GameObject movementVisualizer = null;
	public LayerMask levelLayerMask;

	// Public variables
	[HideInInspector] public Vector3 spawnPoint;
	[HideInInspector] public Vector3 velocity;

	// Private variables
	private float nextJumpTime;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		player = GetComponent<Player>();

		nextJumpTime = 0f;
	}

	private void Start()
	{
		// Public variables
		spawnPoint = transform.position;

		// Private variables
		velocity = Vector3.zero;
	}

	private void FixedUpdate()
	{
		if (photonView.IsMine || isOfflinePlayer)
		{
			// Camera rotation
			float mouseX = Input.GetAxis("Mouse X") * mouseAcceleration * Time.fixedDeltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * mouseAcceleration * Time.fixedDeltaTime;
			player.head.transform.localRotation = Quaternion.Euler(player.head.transform.localRotation.eulerAngles.x - mouseY, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);

			ControlMovement();
		}
	}

	private void OnDrawGizmos()
	{
		// velocity visualization
		Vector3 worldVelocity = transform.position + velocity * Time.fixedDeltaTime;

		Gizmos.DrawWireSphere(worldVelocity, 0.25f);
		Gizmos.DrawWireSphere(worldVelocity + Vector3.up * 1.7f, 0.25f);

		Gizmos.DrawLine(worldVelocity + Vector3.forward * 0.25f, worldVelocity + Vector3.forward * 0.25f + Vector3.up * 1.7f);
		Gizmos.DrawLine(worldVelocity - Vector3.forward * 0.25f, worldVelocity - Vector3.forward * 0.25f + Vector3.up * 1.7f);
		Gizmos.DrawLine(worldVelocity + Vector3.right * 0.25f, worldVelocity + Vector3.right * 0.25f + Vector3.up * 1.7f);
		Gizmos.DrawLine(worldVelocity - Vector3.right * 0.25f, worldVelocity - Vector3.right * 0.25f + Vector3.up * 1.7f);
	}

	//--------------------------
	// PlayerMovement methods
	//--------------------------
	private void ControlMovement()
	{
		// Smooth locomotion
		Vector3 forwardMovement = transform.forward * Input.GetAxisRaw("Vertical");
		Vector3 sidewaysMovement = transform.right * Input.GetAxisRaw("Horizontal");

		// Gravity
		ApplyGravity();

		// Movement acceleration
		if (forwardMovement != Vector3.zero || sidewaysMovement != Vector3.zero)
		{
			Accelerate((forwardMovement + sidewaysMovement).normalized * maxAcceleration);
		}
		else
		{
			Decelerate(maxDeceleration);
		}

		// Jumping
		if (Input.GetButton("Jump") && Time.time > nextJumpTime) { velocity.y = 10f; nextJumpTime = Time.time + jumpCooldown; }
		
		Move();
		VisualizeMovement();
	}

	private void ApplyGravity()
	{
		velocity += Physics.gravity * Time.fixedDeltaTime;
	}

	private void Accelerate(Vector3 acceleration)
	{
		

		velocity += Vector3.ClampMagnitude(acceleration, maxAcceleration);
		if (velocity.magnitude > maxSpeed)
		{
			float vy = velocity.y;
			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
			velocity.y = vy;
		}
	}

	private void Decelerate(float deceleration)
	{
		float vy = velocity.y;

		deceleration = Mathf.Clamp(deceleration, 0, maxDeceleration);

		velocity = Vector3.ClampMagnitude(velocity, Mathf.Max(0, velocity.magnitude - deceleration));
		velocity.y = vy;
	}

	private void Move()
	{
		transform.position += velocity * Time.fixedDeltaTime;

		// collision detection
		RaycastHit hit;
		if (velocity.y < 0 && Physics.SphereCast(transform.position + Vector3.up * 1f, 0.5f, Vector3.down, out hit, 0.6f, levelLayerMask))
		{
			velocity.y = 0;
			transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
		}
	}

	private void VisualizeMovement()
	{
		// rolling sphere
		movementVisualizer.transform.localScale = Vector3.one * (velocity.magnitude / maxSpeed);
		movementVisualizer.transform.Rotate(Input.GetAxisRaw("Vertical") * velocity.magnitude, 0, 0); //velocity.z
	}

	//private void OldMovement()
	//{


	//	// Rotating towards camera
	//	//direction = Quaternion.Euler(0, headTransform.rotation.eulerAngles.y, 0) * direction;

	//	// Sticking to slopes
	//	if ((velocity.x != 0 || velocity.z != 0) && velocity.y <= 0)
	//	{
	//		RaycastHit hit;
	//		if (Physics.Raycast(transform.position, Vector3.down, out hit, _characterController.height / 2 + 0.0001f))
	//			velocity += Vector3.down * 0.2f; // slope magnet force
	//	}
	//}

	[PunRPC]
	private bool Jump()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
		{
			GameObject jumpingPlatfiorm = Instantiate(jumpingPlatfiormPrafab);
			jumpingPlatfiorm.GetComponent<PlayerJumpPlatform>().Shoot(hit.point);

			//velocity += new Vector3(0, sprintJumpingPower, 0);
			return true;
		}
		else
			return false;
	}
}