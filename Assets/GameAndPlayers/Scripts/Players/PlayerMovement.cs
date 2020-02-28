﻿using System.Collections;
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
	[Header("Movement")]
	public float walkingMaximumSpeed = 5f;
	public float sprintingMaximumSpeed = 11f;
	public float floatingMaximumSpeed = 2f;
	public float maxGroundAcceleration = 1f;
	public float maxAirAcceleration = 0.5f;
	public float groundDeceleration = 2f;
	public float airDeceleration = 2f;
	public float jumpCooldown = 1f;
	[Header("Physics")]
	public float mass = 2f;
	public float bounciness = 1f;
	public float maxSlope = 30f;
	[Header("Camera")]
	public float mouseAcceleration = 100f;
	[Header("Prefabs")]
	public GameObject jumpingPlatfiormPrafab = null;
	[Header("Game objects")]
	public GameObject movementVisualizer = null;
	public GameObject groundVisualizer = null;
	public LayerMask levelLayerMask;

	// Public variables
	[HideInInspector] public Vector3 spawnPoint;
	[HideInInspector] public float verticalVelocity;
	[HideInInspector] public Vector2 horizontalVelocity;
	[HideInInspector] public bool isGrounded;

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
	private float _concuction;

	// Accessors
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

		Vector3 origin = transform.position + Vector3.up * (height / 2);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(origin, Velocity * Time.fixedDeltaTime);

		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(center1Velocity, radius);
		Gizmos.DrawWireSphere(center2Velocity, radius);
		Gizmos.DrawLine(center1Velocity + Vector3.forward * radius, center2Velocity + Vector3.forward * radius);
		Gizmos.DrawLine(center1Velocity - Vector3.forward * radius, center2Velocity - Vector3.forward * radius);
		Gizmos.DrawLine(center1Velocity + Vector3.right * radius, center2Velocity + Vector3.right * radius);
		Gizmos.DrawLine(center1Velocity - Vector3.right * radius, center2Velocity - Vector3.right * radius);

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

		// Gravity
		ApplyGravity();

		// Movement acceleration
		float acceleration = 0;

		if ((forwardMovement != Vector3.zero || sidewaysMovement != Vector3.zero) && Concuction <= 0)
		{
			if (isGrounded)
			{
				acceleration = sprintingMaximumSpeed;
				if (!Input.GetButton("Sprint")) acceleration = walkingMaximumSpeed;
			}
			else
				acceleration = floatingMaximumSpeed;

			Vector3 direction = forwardMovement + sidewaysMovement;
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

		// Jumping
		if (Input.GetButton("Jump") && Time.time > nextJumpTime && isGrounded) { Jump(); nextJumpTime = Time.time + jumpCooldown; }
		
		Move();
		VisualizeMovement();
	}

	private void ApplyGravity()
	{
		Velocity += Physics.gravity * mass * Time.fixedDeltaTime;
	}

	private void Accelerate(Vector2 direction, float targetSpeed)
	{
		if (isGrounded)
		{
			direction = Vector2.ClampMagnitude(direction.normalized * targetSpeed, maxGroundAcceleration); //  * Concuction
			horizontalVelocity += direction;
		}
		else
		{
			float mag = horizontalVelocity.magnitude;
			direction = Vector2.ClampMagnitude(direction.normalized * targetSpeed, maxAirAcceleration); //  * Concuction
			horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity + direction, Mathf.Max(Mathf.Max(floatingMaximumSpeed, maxAirAcceleration), mag));
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
		// setting is grounded to false expecting this function to change that if the character is grounded
		isGrounded = false;
		
		// collision detection
		float radius = 0.25f;
		float height = 2f;

		RaycastHit hit;
		float rayLength;
		Vector3 center1;
		Vector3 center2;

		// vertical
		rayLength = Mathf.Abs(verticalVelocity * Time.fixedDeltaTime) + radius * 2;

		if (verticalVelocity < 0) // down
		{
			center1 = transform.position + Vector3.up * (radius * 2);

			if (Physics.SphereCast(center1, radius, Vector3.down, out hit, rayLength, levelLayerMask))
			{
				float hitPointY = hit.point.y;

				groundVisualizer.transform.position = hit.point;

				verticalVelocity = 0;
				if (Vector3.Angle(Vector3.up, hit.normal) <= maxSlope)
					transform.position = new Vector3(transform.position.x, hitPointY, transform.position.z);

				isGrounded = true;
			}
		}
		else if (verticalVelocity > 0) // up
		{
			center1 = transform.position + Vector3.up * (height - radius * 2);

			if (Physics.SphereCast(center1, radius, Vector3.up, out hit, rayLength, levelLayerMask))
			{
				float hitPointY = hit.point.y;

				verticalVelocity = 0;
				transform.position = new Vector3(transform.position.x, hitPointY - height, transform.position.z);
			}
		}

		// horizontal
		float magnitude = horizontalVelocity.magnitude * Time.fixedDeltaTime;
		if (magnitude != 0)
		{
			Vector3 direction = new Vector3(horizontalVelocity.x, 0, horizontalVelocity.y).normalized;
			float castingOffset = radius;

			rayLength = Mathf.Abs(magnitude) + Mathf.Abs(castingOffset);
			center1 = transform.position + Vector3.up * (radius * 2) - direction * castingOffset;
			center2 = transform.position + Vector3.up * (height - radius * 2) - direction * castingOffset;

			if (Physics.CapsuleCast(center1, center2, radius, direction, out hit, rayLength, levelLayerMask))
			{
				horizontalVelocity = (horizontalVelocity.normalized + new Vector2(hit.normal.x, hit.normal.z)).normalized * Mathf.Abs(magnitude) * bounciness;
				Concuction = 0.2f;
			}
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