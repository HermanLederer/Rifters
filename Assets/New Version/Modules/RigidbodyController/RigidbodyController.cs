using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviour
{
	//
	// Other components
	#region Other components
	public CapsuleCollider Col { get; private set; }
	public Rigidbody Rb { get; private set; }
	#endregion

	//
	// Editor variables
	#region Editor variables
	[Header("Movement")]
	public float maxSprintingSpeed = 12f;
	//public float maxWalkingSpeed = 5f;
	public float maxFloatingSpeed = 2f;

	[Range(0f, 200f)] public float maxGroundAcceleration = 20f;
	[Range(0f, 200f)] public float maxGroundDeceleration = 10f;

	[Range(0f, 200f)] public float maxAirAcceleration = 5f;
	[Range(0f, 200f)] public float maxAirDeceleration = 5f;

	public float jumpPower = 8f;
	public float stepDistance;
	public float stepRayDistance;
	public float stepVelocity;

	[Header("Physics")]
	[Range(0f, 2f)] public float groundMagnetism = 0.5f;
	public LayerMask levelLayerMask;

	[Header("Movement axies")]
	[HideInInspector] public float axisV;
	[HideInInspector] public float axisH;
	#endregion

	//
	// Private variables
	#region Private variables;
	public bool isGrounded { get; private set; }
	private float groundMagnetismForce = 0f;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		//player = GetComponent<Player>();
		Col = GetComponent<CapsuleCollider>();
		Rb = GetComponent<Rigidbody>();

		isGrounded = true;

		Rb.centerOfMass = new Vector3(0, -1, 0);
	}

	private void FixedUpdate()
	{
		ControlMovement();
	}

    //--------------------------
    // PlayerMovement methods
    //--------------------------
    private void ControlMovement()
	{
		// Climbing the ground
		FollowGround();

		// Acceleration
		Vector3 horizontalVelocity = new Vector3(Rb.velocity.x, 0, Rb.velocity.z);

		float targetSpeed = 0;

		if ((axisV != 0 || axisH != 0))
		{
			// calculating direction vector
			Vector3 forwardMovement = transform.forward * axisV;
			Vector3 sidewaysMovement = transform.right * axisH;
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
	}

	private void Accelerate(Vector3 direction, float acceleration)
	{
		if (isGrounded) Rb.AddForce(direction * acceleration * Rb.mass, ForceMode.Impulse);
		else
		{
			float mag = Mathf.Max(new Vector3(Rb.velocity.x, 0, Rb.velocity.z).magnitude, maxFloatingSpeed);
			Rb.AddForce(direction * acceleration * Rb.mass, ForceMode.Impulse);

			Vector3 vel = new Vector3(Rb.velocity.x, 0, Rb.velocity.z);
			vel = Vector3.ClampMagnitude(vel, mag);

			Rb.velocity = new Vector3(vel.x, Rb.velocity.y, vel.z);
		}
	}

	private void Decelerate(float deceleration)
	{
		// Only decelerates horizontally
		Vector3 horizontalVelocity = new Vector3(Rb.velocity.x, 0, Rb.velocity.z);
		horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, horizontalVelocity.magnitude - deceleration);
		Rb.velocity = new Vector3(horizontalVelocity.x, Rb.velocity.y, horizontalVelocity.z);
	}

	private void FollowGround()
	{
		isGrounded = false;

		// Params
		float rayLength = 0.2f;
		Vector3 center = transform.position;
		center += Vector3.up * (rayLength * 0.5f);
		center += Vector3.up * Col.radius;

		Debug.DrawRay(center, Vector3.down * rayLength);

		RaycastHit hit;
		if (Physics.SphereCast(center, Col.radius, Vector3.down, out hit, rayLength, levelLayerMask))
		{
			isGrounded = true;
			groundMagnetismForce = 0f;
		}
	}

	private void ApplyGroundMagnetism()
	{
		if (!isGrounded)
		{
			// Params
			float rayLength = 12f;
			Vector3 center = transform.position;

			RaycastHit hit;
			float downAngle = Vector3.Angle(Vector3.down, Rb.velocity);
			if (downAngle < 90)
			{
				float groundMagnetismAmount = 1f;

				if (Physics.Raycast(center, Vector3.down, out hit, rayLength, levelLayerMask))
				{
					groundMagnetismAmount += (1 - hit.distance / rayLength);
				}

				groundMagnetismForce += groundMagnetismAmount * Time.fixedDeltaTime;
				Rb.AddForce(Vector3.down * groundMagnetismForce * groundMagnetism * Rb.mass, ForceMode.Impulse);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + Vector3.up * 0.05f, transform.position + transform.forward * stepRayDistance + Vector3.up * 0.05f);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position + Vector3.up * stepDistance, transform.position + transform.forward * .6f + Vector3.up * stepDistance);
	}

	public bool Jump()
	{
		if (!isGrounded) return false;

		groundMagnetismForce = 0f;
		Rb.AddForce(Vector3.up * jumpPower * Rb.mass, ForceMode.Impulse);
		return true;
	}
}