using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviourPun
{
	//
	// Other components
	#region Other components
	public Player player { get; private set; }
	#endregion

	//
	// Editor Variables
	#region Editor variables
	[Header("Movement")]
	public float maxSprintingSpeed = 12f;
	public float maxWalkingSpeed = 5f;
	public float maxFloatingSpeed = 2f;
	public float maxGroundAcceleration = 1f;
	public float maxAirAcceleration = 0.5f;
	public float groundDeceleration = 2f;
	public float airDeceleration = 2f;
	public float slopeSlideAcceleration = 2f;
	public float maxWalkingSlope = 35f;
	public float maxStepuDistance = 1f;
	public float jumpCooldown = 1f;

	[Header("Collider")]
	public float height = 2f;
	public float radius = 0.25f;

	[Header("Physics")]
	public float mass = 2f;
	[Range(0f, 2f)] public float bounciness = 1f;

	[Header("Camera")]
	public float mouseAcceleration = 100f;

	[Header("Prefabs")]
	public GameObject jumpingPlatfiormPrafab = null;

	[Header("Game objects")]
	public GameObject movementVisualizer = null;
	public GameObject groundVisualizer = null;
	public GameObject slopeObject = null;
	public LayerMask levelLayerMask;
	#endregion

	//
	// Public variables
	#region Public variables
	[HideInInspector] public Vector3 spawnPoint;
	[HideInInspector] public float verticalVelocity;
	[HideInInspector] public Vector2 horizontalVelocity;
	[HideInInspector] public bool isGrounded;
	#endregion

	//
	// Private variables
	#region Private variables
	private float nextJumpTime;
	private float _concuction;
	private float slope;
	private Vector3 slopeNormal;
	#endregion

	//
	// Accessors
	#region Accessors
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

	private float Concuction
	{
		get
		{
			return Mathf.Clamp(_concuction - Time.time, 0, 1);
		}
		set
		{
			_concuction = Time.time + value;
		}
	}
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		player = GetComponent<Player>();

		nextJumpTime = 0f;
		_concuction = Time.time;
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
		if (photonView.IsMine || player.isOfflinePlayer)
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
		Vector3 center1 = transform.position + Vector3.up * (radius * 2);
		Vector3 center2 = transform.position + Vector3.up * (height - radius);

		Vector3 center1Velocity = center1 + Velocity * Time.fixedDeltaTime;
		Vector3 center2Velocity = center2 + Velocity * Time.fixedDeltaTime;

		Vector3 origin = transform.position + Vector3.up * (height / 2);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(center1Velocity, radius);
		Gizmos.DrawWireSphere(center2Velocity, radius);
		Gizmos.DrawLine(center1Velocity + Vector3.forward * radius, center2Velocity + Vector3.forward * radius);
		Gizmos.DrawLine(center1Velocity - Vector3.forward * radius, center2Velocity - Vector3.forward * radius);
		Gizmos.DrawLine(center1Velocity + Vector3.right * radius, center2Velocity + Vector3.right * radius);
		Gizmos.DrawLine(center1Velocity - Vector3.right * radius, center2Velocity - Vector3.right * radius);

		//Vector3 direction = new Vector3(horizontalVelocity.x, Mathf.Clamp(verticalVelocity, 0, verticalVelocity), horizontalVelocity.y);
		//direction = Vector3.Cross(Quaternion.Euler(0, 90, 0) * direction, slopeNormal);
		Gizmos.color = Color.white;
		Vector3 direction = Vector3.Cross(Vector3.Cross(transform.up, slopeNormal), slopeNormal);
		//Vector3 direction = Quaternion.Euler(0, 90, 0) * Vector3.Cross(transform.up, slopeNormal);
		Gizmos.DrawRay(origin, direction);

		//RaycastHit hit;
		//float rayLength;
		//float magnitude = horizontalVelocity.magnitude * Time.fixedDeltaTime;
		////if (magnitude != 0)
		////{
		//	Vector3 direction = new Vector3(horizontalVelocity.x, 0 , horizontalVelocity.y).normalized;
		//	float castingOffset = radius;

		//	rayLength = Mathf.Abs(magnitude) + Mathf.Abs(castingOffset);
		//	center1 = transform.position + Vector3.up * (radius * 2) - direction * castingOffset;
		//	center2 = transform.position + Vector3.up * (height - radius * 2) - direction * castingOffset;

		//	if (Physics.CapsuleCast(center1, center2, radius, direction, out hit, rayLength))
		//	{
		//		Gizmos.color = Color.white;
		//		Gizmos.DrawWireSphere(center1, radius);
		//		Gizmos.DrawWireSphere(center2, radius);

		//		Gizmos.color = Color.red;
		//		Gizmos.DrawLine(origin, hit.point);
		//	}
		////}
	}

	//--------------------------
	// PlayerMovement methods
	//--------------------------
	private void ControlMovement()
	{
		// Smooth locomotion
		Vector3 forwardMovement = transform.forward * Input.GetAxisRaw("Vertical");
		Vector3 sidewaysMovement = transform.right * Input.GetAxisRaw("Horizontal");

		// Movement acceleration
		float acceleration = 0;
		Vector3 groundDirection = Quaternion.Euler(0, 90, 0) * Vector3.Cross(transform.up, slopeNormal);
		Vector3 direction = forwardMovement + sidewaysMovement;

		if ((forwardMovement != Vector3.zero || sidewaysMovement != Vector3.zero) && Concuction <= 0)
		{
			if (isGrounded)
			{
				acceleration = maxSprintingSpeed;
				if (!Input.GetButton("Sprint")) acceleration = maxWalkingSpeed;
			}
			else
				acceleration = maxFloatingSpeed;
			
			if (slope > maxWalkingSlope && Vector3.Angle(direction, groundDirection) < 45)
				acceleration = 0f;

			Accelerate(new Vector2(direction.x, direction.z), acceleration);
		}
		else
		{
			if (!isGrounded) Decelerate(airDeceleration);
		}

		if (horizontalVelocity.magnitude > acceleration && isGrounded)
		{
			float deceleration = groundDeceleration;
			float overhead = horizontalVelocity.magnitude - acceleration;
			Decelerate(Mathf.Clamp(overhead, 0, Mathf.Max(deceleration, deceleration * acceleration)), true);
		}

		 ApplyGravity();

		// Jumping
		if (Input.GetButton("Jump") && Time.time > nextJumpTime && isGrounded) { Jump(); nextJumpTime = Time.time + jumpCooldown; }
		
		Move();
		VisualizeMovement();
	}

	private void ApplyGravity()
	{
		if (isGrounded && slope > maxWalkingSlope)
		{
			// on slopes
			Vector3 direction = Vector3.Cross(Vector3.Cross(transform.up, slopeNormal), slopeNormal);
			Velocity += direction * slopeSlideAcceleration;
		}
		else
		{
			// constant
			Velocity += Physics.gravity * mass * Time.fixedDeltaTime;
		}
	}

	private void Accelerate(Vector3 direction, float targetSpeed)
	{
		if (isGrounded)
		{
			direction = Vector3.ClampMagnitude(direction.normalized * targetSpeed, maxGroundAcceleration);
			Velocity += direction;
		}
		else
		{
			float mag = horizontalVelocity.magnitude;
			direction = Vector3.ClampMagnitude(direction.normalized * targetSpeed, maxAirAcceleration);
			Velocity = Vector3.ClampMagnitude(Velocity + direction, Mathf.Max(Mathf.Max(maxFloatingSpeed, maxAirAcceleration), mag));
		}
	}

	private void Accelerate(Vector2 direction, float targetSpeed)
	{
		if (isGrounded)
		{
			direction = Vector2.ClampMagnitude(direction.normalized * targetSpeed, maxGroundAcceleration);
			horizontalVelocity += direction;
		}
		else
		{
			float mag = horizontalVelocity.magnitude;
			direction = Vector2.ClampMagnitude(direction.normalized * targetSpeed, maxAirAcceleration);
			horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity + direction, Mathf.Max(Mathf.Max(maxFloatingSpeed, maxAirAcceleration), mag));
		}
	}

	private void Decelerate(float deceleration, bool force = false)
	{
		if (!force)
			if (isGrounded)
				deceleration = Mathf.Clamp(deceleration, 0, groundDeceleration);
			else
				deceleration = Mathf.Clamp(deceleration, 0, airDeceleration);

		horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, Mathf.Max(0, horizontalVelocity.magnitude - deceleration));
	}

	private void Move()
	{
		//
		// Collision detection
		#region Collision detection
		RaycastHit hit;
		float rayLength;
		float magnitude;
		Vector3 direction;
		float castingOffsetLength;
		Vector3 center1;
		Vector3 center2;
		Vector3 originalPosition = transform.position;

		// Ground detector
		#region Ground detector
		isGrounded = false;
		
		if (verticalVelocity <= 0)
		{
			direction = Vector3.down;
			castingOffsetLength = height / 2;
			magnitude = verticalVelocity * Time.fixedDeltaTime;
			rayLength = Mathf.Abs(magnitude) + Mathf.Abs(castingOffsetLength);
			rayLength = Mathf.Max(rayLength, 0.3f);
			center1 = transform.position + Velocity * Time.fixedDeltaTime + Vector3.up * (radius) - direction * castingOffsetLength;
			//center1 = transform.position + Vector3.up * (radius) - direction * castingOffsetLength;

			// thatying on the ground
			if (Physics.SphereCast(center1, radius, direction, out hit, rayLength, levelLayerMask))
			{
				slope = Vector3.Angle(Vector3.up, hit.normal);
				slopeNormal = hit.normal;

				float hitPointY = hit.point.y - radius * (Vector3.Angle(Vector3.up, hit.normal) / 90);
				// staying on the ground
				transform.position = new Vector3(transform.position.x, hitPointY, transform.position.z);
				verticalVelocity = 0;

				isGrounded = true;
			}
		}
		#endregion

		// Bumper
		#region Bumper
		magnitude = Velocity.magnitude * Time.fixedDeltaTime;
		direction = new Vector3(horizontalVelocity.x, Mathf.Clamp(verticalVelocity, 0, verticalVelocity), horizontalVelocity.y).normalized;
		direction = Vector3.Cross(Quaternion.Euler(0, 90, 0) * direction, slopeNormal);
		castingOffsetLength = magnitude;

		rayLength = radius + castingOffsetLength;
		center1 = transform.position + Vector3.up * (radius * 2) - direction * castingOffsetLength;
		center2 = transform.position + Vector3.up * (height - radius) - direction * castingOffsetLength;

		if (Physics.CapsuleCast(center1, center2, radius, direction, out hit, rayLength, levelLayerMask))
		{
			Velocity = -(2 * (Vector3.Dot(Velocity, Vector3.Normalize(hit.normal))) * Vector3.Normalize(hit.normal) - Velocity) * bounciness;
			Concuction = 0.2f;
		}
		#endregion

		#endregion

		//
		// Movement
		transform.position += Velocity * Time.fixedDeltaTime;
	}

	private void VisualizeMovement()
	{
		// rolling sphere
		//movementVisualizer.transform.localScale = Vector3.one * (Velocity.magnitude / maxSpeed);
		movementVisualizer.transform.Rotate(Input.GetAxisRaw("Vertical") * Velocity.magnitude, 0, 0);
	}

	[PunRPC]
	private bool Jump()
	{
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