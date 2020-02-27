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

	public float walkingTargetSpeed = 4f;
	public float sprintingTargetSpeed = 7f;
	public float maxAcceleration = 1f;
	public float maxDeceleration = 2f;

	public float jumpCooldown = 1f;
	public bool isGrounded;

	public float mouseAcceleration = 100f;

	public GameObject jumpingPlatfiormPrafab = null;

	public GameObject movementVisualizer = null;
	public GameObject groundVisualizer = null;
	public LayerMask levelLayerMask;

	// Public variables
	[HideInInspector] public Vector3 spawnPoint;
	[HideInInspector] public float verticalVelocity;
	[HideInInspector] public Vector2 horizontalVelocity;

	public Vector3 Velocity
	{
		get
		{
			return new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.y);
		}

		set
		{
			verticalVelocity = value.y;
			horizontalVelocity = new Vector2(value.x, value.z);
		}
	}

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
		Velocity = Vector3.zero;
	}

	private void FixedUpdate()
	{
		if (photonView.IsMine || isOfflinePlayer)
		{
			// Controls
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
		Vector3 worldVelocity = transform.position + Velocity * Time.fixedDeltaTime;

		float radius = 0.25f;
		float height = 2f;
		Vector3 center = transform.position + Vector3.up * ((height + radius) / 2);
		Vector3 center1 = transform.position + Vector3.up * radius;
		Vector3 center2 = center1 + Vector3.up * (height - radius * 2);

		Vector3 center1Velocity = center1 + Velocity * Time.fixedDeltaTime;
		Vector3 center2Velocity = center2 + Velocity * Time.fixedDeltaTime;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(center, radius);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(center1Velocity, radius);
		Gizmos.DrawWireSphere(center2Velocity, radius);

		Gizmos.DrawLine(center1Velocity + Vector3.forward * radius, center2Velocity + Vector3.forward * radius);
		Gizmos.DrawLine(center1Velocity - Vector3.forward * radius, center2Velocity - Vector3.forward * radius);
		Gizmos.DrawLine(center1Velocity + Vector3.right * radius, center2Velocity + Vector3.right * radius);
		Gizmos.DrawLine(center1Velocity - Vector3.right * radius, center2Velocity - Vector3.right * radius);
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
			float speed = sprintingTargetSpeed;
			if (!Input.GetButton("Sprint")) speed = walkingTargetSpeed;

			Vector3 direction = forwardMovement + sidewaysMovement;
			Accelerate(new Vector2(direction.x, direction.z), speed);
		}
		else
		{
			Decelerate(maxDeceleration);
		}

		// Jumping
		if (Input.GetButton("Jump") && Time.time > nextJumpTime && isGrounded) { Jump(); nextJumpTime = Time.time + jumpCooldown; }
		
		Move();
		VisualizeMovement();
	}

	private void ApplyGravity()
	{
		Velocity += Physics.gravity * Time.fixedDeltaTime;
	}

	private void Accelerate(Vector2 direction, float targetSpeed)
	{
		horizontalVelocity += Vector2.ClampMagnitude(direction.normalized * targetSpeed, maxAcceleration);
		horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, targetSpeed);
	}

	private void Decelerate(float deceleration)
	{
		float vy = verticalVelocity;

		deceleration = Mathf.Clamp(deceleration, 0, maxDeceleration);

		Velocity = Vector3.ClampMagnitude(Velocity, Mathf.Max(0, Velocity.magnitude - deceleration));
		verticalVelocity = vy;
	}

	private void Move()
	{
		// aetting is grounded to false expecting this function to change that if the character is grounded
		isGrounded = false;
		
		// collision detection
		float radius = 0.25f;
		float height = 2f;

		RaycastHit hit;
		float rayLength;
		Vector3 center;

		// vertical
		rayLength = Mathf.Abs(verticalVelocity * Time.fixedDeltaTime) + radius * 2;

		if (verticalVelocity < 0) // down
		{
			center = transform.position + Vector3.up * (radius * 2);

			if (Physics.SphereCast(center, radius, Vector3.down, out hit, rayLength, levelLayerMask))
			{
				float hitPointY = hit.point.y;

				groundVisualizer.transform.position = hit.point;

				verticalVelocity = 0;
				transform.position = new Vector3(transform.position.x, hitPointY, transform.position.z);
				isGrounded = true;
			}
		}
		else if (verticalVelocity > 0) // up
		{
			center = transform.position + Vector3.up * (height - radius * 2);

			if (Physics.SphereCast(center, radius, Vector3.up, out hit, rayLength,levelLayerMask))
			{
				float hitPointY = hit.point.y;

				verticalVelocity = 0;
				transform.position = new Vector3(transform.position.x, hitPointY - height, transform.position.z);
			}
		}

		// horizontal x
		if (horizontalVelocity.x != 0)
		{
			Vector3 direction = new Vector3(horizontalVelocity.x, 0, 0);
			float castingOffset = Mathf.Clamp(horizontalVelocity.x, -radius, radius);

			rayLength = Mathf.Abs(horizontalVelocity.x * Time.fixedDeltaTime) + Mathf.Abs(castingOffset);
			center = transform.position + Vector3.up * ((height + radius) / 2) - Vector3.right * castingOffset;

			if (Physics.SphereCast(center, radius, direction, out hit, rayLength, levelLayerMask))
			{
				horizontalVelocity.x = 0;
				Debug.Log(Mathf.Abs(Vector3.Angle(Vector3.forward, hit.normal) / 90 - 1));
			}
		}

		// horizontal 
		if (horizontalVelocity.y != 0)
		{
			Vector3 direction = new Vector3(0, 0, horizontalVelocity.y);
			float castingOffset = Mathf.Clamp(horizontalVelocity.y, -radius, radius);

			rayLength = Mathf.Abs(horizontalVelocity.y * Time.fixedDeltaTime) + Mathf.Abs(castingOffset);
			center = transform.position + Vector3.up * ((height + radius) / 2) - Vector3.forward * castingOffset;

			if (Physics.SphereCast(center, radius, direction, out hit, rayLength, levelLayerMask))
				horizontalVelocity.y = 0;
		}

		// Movement
		transform.position += Velocity * Time.fixedDeltaTime;
	}

	private void VisualizeMovement()
	{
		// rolling sphere
		//movementVisualizer.transform.localScale = Vector3.one * (Velocity.magnitude / maxSpeed);
		//movementVisualizer.transform.Rotate(Input.GetAxisRaw("Vertical") * Velocity.magnitude, 0, 0);
	}

	[PunRPC]
	private bool Jump()
	{
		float radius = 0.25f;
		Vector3 center = transform.position + Vector3.up * radius;

		RaycastHit hit;
		if (Physics.Raycast(center, -transform.up, out hit, 2f))
		{
			GameObject jumpingPlatfiorm = Instantiate(jumpingPlatfiormPrafab);
			jumpingPlatfiorm.GetComponent<PlayerJumpPlatform>().Shoot(hit.point);

			return true;
		}
		else
			return false;
	}
}