using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviourPun
{
	//
	// Other components
	#region Other components
	private SphereCollider collider;
	private Rigidbody rigidbody;
	#endregion

	//
	// Editor Variables
	#region Editor variables
	public Player player;
	[Header("Movement")]
	public float maxSprintingSpeed = 12f;
	//public float maxWalkingSpeed = 5f;
	public float maxFloatingSpeed = 2f;

	[Range(0f, 50f)] public float maxGroundAcceleration = 20f;
	[Range(0f, 50f)] public float maxGroundDeceleration = 10f;

	[Range(0f, 50f)] public float maxAirAcceleration = 5f;
	[Range(0f, 50f)] public float maxAirDeceleration = 5f;

	public float jumpPower = 8f;
	public float jumpCooldown = 0.1f;

	[Range(0f, 2f)] public float groundMagnetism = 0.5f;

	[Header("Prefabs")]
	public GameObject jumpingPlatfiormPrafab = null;
	#endregion

	//
	// Public variables
	#region Public variables
	[HideInInspector] public Vector3 spawnPoint;
	[HideInInspector] public Vector2 direction;
	#endregion

	//
	// Private variables
	#region Private variables
	private float nextJumpTime;
	private Vector3 groundNormal;
	private bool isGrounded = true;
	private float groundMagnetismForce = 0f;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		//player = GetComponent<Player>();
		collider = GetComponent<SphereCollider>();
		rigidbody = GetComponent<Rigidbody>();

		nextJumpTime = 0f;
	}

	private void Start()
	{
		// Public variables
		spawnPoint = transform.position;
	}

	private void FixedUpdate()
	{
		//if (photonView.IsMine || player.isOfflinePlayer)
		//{
			// Controls
			ControlMovement();
		//}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!isGrounded)
		{
			Vector3 normal = collision.contacts[0].normal;

			if (Vector3.Angle(Vector3.up, normal) >= 90)
			{
				//Debug.DrawRay(collision.contacts[0].point, normal, Color.red, 2f);
				rigidbody.AddForce(normal * rigidbody.velocity.magnitude * rigidbody.mass * collider.material.bounciness, ForceMode.Impulse);
			}
		}
	}

	//--------------------------
	// PlayerMovement methods
	//--------------------------
	private void ControlMovement()
	{
		// Direction
		float axisV = Input.GetAxisRaw("Vertical");
		float axisH = Input.GetAxisRaw("Horizontal");

		// Climbing the ground
		FollowGround();

		// Acceleration
		Vector3 horizontalVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

		float targetSpeed = 0;

		if ((axisV != 0 || axisH != 0))
		{
			Debug.DrawRay(transform.position, Vector3.up * 10f) ;

			// calculating direction vector
			Vector3 forwardMovement = player.orientationTransform.forward * axisV;
			Vector3 sidewaysMovement = player.orientationTransform.right * axisH;
			Vector3 targetDirection = forwardMovement + sidewaysMovement;
			targetDirection = targetDirection.normalized;
			

			// determining acceleration force and target speed
			float acceleration;
			if (isGrounded)
			{
				acceleration = maxGroundAcceleration;
				targetSpeed = maxSprintingSpeed;
				//if (!Input.GetButton("Sprint")) targetSpeed = maxWalkingSpeed;
			}
			else
			{
				acceleration = maxAirAcceleration;
				targetSpeed = maxFloatingSpeed;
			}

			// accelerating
			Accelerate(targetDirection, acceleration * Time.fixedDeltaTime);
		}

		// Deceleration
		float deceleration;
		if (isGrounded) deceleration = maxGroundDeceleration;
		else deceleration = maxAirDeceleration;
		deceleration *= Time.fixedDeltaTime;

		float overhead = horizontalVelocity.magnitude - targetSpeed;
		Decelerate(Mathf.Clamp(overhead, 0, Mathf.Max(deceleration, deceleration * targetSpeed)));

		// Ground magnetism
		ApplyGroundMagnetism();

		// Jumping
		if (isGrounded && Time.time >= nextJumpTime && Input.GetButton("Jump")) { Jump(); nextJumpTime = Time.time + jumpCooldown; }
	}

	private void LateUpdate()
	{
		// Moving the model
		player.characterModel.transform.position = player.playerOrigin.position + Vector3.down * collider.radius;
	}

	private void Accelerate(Vector3 direction, float acceleration)
	{
		if (isGrounded) rigidbody.AddForce(direction * acceleration * rigidbody.mass, ForceMode.Impulse);
		else
		{
			float mag = Mathf.Max(new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude, maxFloatingSpeed);
			rigidbody.AddForce(direction * acceleration * rigidbody.mass, ForceMode.Impulse);

			Vector3 vel = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
			vel = Vector3.ClampMagnitude(vel, mag);

			rigidbody.velocity = new Vector3(vel.x, rigidbody.velocity.y, vel.z);
		}
	}

	private void Decelerate(float deceleration)
	{
		// Only decelerates horizontally
		Vector3 horizontalVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, horizontalVelocity.magnitude - deceleration);
		rigidbody.velocity = new Vector3(horizontalVelocity.x, rigidbody.velocity.y, horizontalVelocity.z);
	}

	private void FollowGround()
	{
		isGrounded = false;

		// Params
		float rayLength = collider.radius + 0.1f;
		Vector3 center = transform.position + Vector3.up * collider.radius;

		RaycastHit hit;
		if (Physics.SphereCast(center, collider.radius, Vector3.down, out hit, rayLength))
		{
			isGrounded = true;
			groundNormal = hit.normal;
			groundMagnetismForce = 0f;
			Debug.DrawLine(transform.position + Vector3.down * collider.radius, hit.point, Color.cyan);

			//if (transform.position.y < hit.point.y + collider.radius)
			//	if (Vector3.Angle(Vector3.up, hit.normal) < 50)
			//		transform.position = new Vector3(transform.position.x, hit.point.y + collider.radius, transform.position.z);
		}
	}

	private void ApplyGroundMagnetism()
	{
		// Params
		float rayLength = 12f + 0.1f;
		Vector3 center = transform.position + Vector3.up * collider.radius;

		RaycastHit hit;
		float downAngle = Vector3.Angle(Vector3.down, rigidbody.velocity);
		if (downAngle < 90)
		{
			float groundMagnetismAmount = 1f;

			if (Physics.Raycast(center, Vector3.down, out hit, rayLength))
			{
				//Debug.DrawRay(center, Vector3.down * (1 - hit.distance / rayLength), Color.cyan);
				groundMagnetismAmount += (1 - hit.distance / rayLength);
			}

			groundMagnetismForce += groundMagnetismAmount * Time.fixedDeltaTime;
			rigidbody.AddForce(Vector3.down * groundMagnetismForce * groundMagnetism * rigidbody.mass, ForceMode.Impulse);
		}
	}
	
	private bool Jump()
	{
		groundMagnetism = 0f;
		rigidbody.AddForce(Vector3.up * jumpPower * rigidbody.mass, ForceMode.Impulse);
		return true;
	}
}