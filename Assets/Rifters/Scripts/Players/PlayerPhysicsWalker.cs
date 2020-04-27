using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerPhysicsWalker : MonoBehaviourPun
{
	//
	// Other components
	#region Other components
	new public CapsuleCollider collider { get; private set; }
	new public Rigidbody rigidbody { get; private set; }
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

	public LayerMask levelLayerMask;

	[HideInInspector] public float axisV;
	[HideInInspector] public float axisH;
	#endregion

	//
	// Private variables
	#region Private variables
	private float nextJumpTime;
	private Vector3 groundNormal;
	public bool isGrounded { get; private set; }
	private float groundMagnetismForce = 0f;
	private string horizontalKey;
	private string verticalKey;
	private string jumpKey;
	private float spellTime = -1f;

	
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		// Other components
		//player = GetComponent<Player>();
		collider = GetComponent<CapsuleCollider>();
		rigidbody = GetComponent<Rigidbody>();

		nextJumpTime = 0f;
		isGrounded = true;

		rigidbody.centerOfMass = new Vector3(0, -1, 0);

		if (player.isPlayer1)
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

	}

	private void FixedUpdate()
	{
		//if (photonView.IsMine || player.isOfflinePlayer)
		//{
			// Controls
			ControlMovement();
		//}
	}

    #region Getters and Setters

	public void SetSpellTime(float time)
	{
		spellTime = time;
	}
    #endregion

    //private void OnCollisionEnter(Collision collision)
    //{
    //	if (!isGrounded)
    //	{
    //		Vector3 normal = collision.contacts[0].normal;

    //		if (Vector3.Angle(Vector3.up, normal) >= 90)
    //		{
    //			//Debug.DrawRay(collision.contacts[0].point, normal, Color.red, 2f);
    //			rigidbody.AddForce(normal * rigidbody.velocity.magnitude * rigidbody.mass * collider.material.bounciness, ForceMode.Impulse);
    //		}
    //	}
    //}

    //--------------------------
    // PlayerMovement methods
    //--------------------------
    private void ControlMovement()
	{
		//If the player is casting the spell he can't move
		if (spellTime > 0)
		{
			spellTime -= Time.deltaTime;
			axisV = 0f;
			axisH = 0f;
		}
		else
		{
			axisV = Input.GetAxisRaw(verticalKey);
			axisH = Input.GetAxisRaw(horizontalKey);
		}

		// Climbing the ground
		FollowGround();

		// Acceleration
		Vector3 horizontalVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

		float targetSpeed = 0;

		if ((axisV != 0 || axisH != 0))
		{
			// calculating direction vector
			Vector3 forwardMovement = player.playerOrigin.forward * axisV;
			Vector3 sidewaysMovement = player.playerOrigin.right * axisH;
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
		if (isGrounded && Time.time >= nextJumpTime && Input.GetButton(jumpKey)) { Jump(); nextJumpTime = Time.time + jumpCooldown; }
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
		float rayLengthHalf = collider.radius + 0.1f;
		Vector3 center = player.playerOrigin.position + Vector3.up * rayLengthHalf;

		RaycastHit hit;
		if (Physics.SphereCast(center, collider.radius, Vector3.down, out hit, rayLengthHalf * 2, levelLayerMask))
		{
			Debug.Log("Is grounded entra");
			isGrounded = true;
			groundNormal = hit.normal;
			groundMagnetismForce = 0f;

			//if (transform.position.y < hit.point.y + collider.radius)
			//	if (Vector3.Angle(Vector3.up, hit.normal) < 50)
			//		transform.position = new Vector3(transform.position.x, hit.point.y + collider.radius, transform.position.z);
		}
		Debug.Log("Is grounded: " + isGrounded);
	}

	private void ApplyGroundMagnetism()
	{
		if (!isGrounded)
		{
			// Params
			float rayLength = 12f;
			Vector3 center = player.playerOrigin.position;

			RaycastHit hit;
			float downAngle = Vector3.Angle(Vector3.down, rigidbody.velocity);
			if (downAngle < 90)
			{
				float groundMagnetismAmount = 1f;

				if (Physics.Raycast(center, Vector3.down, out hit, rayLength, levelLayerMask))
				{
					groundMagnetismAmount += (1 - hit.distance / rayLength);
				}

				groundMagnetismForce += groundMagnetismAmount * Time.fixedDeltaTime;
				rigidbody.AddForce(Vector3.down * groundMagnetismForce * groundMagnetism * rigidbody.mass, ForceMode.Impulse);
			}
		}
	}
	
	private bool Jump()
	{
		groundMagnetismForce = 0f;
		rigidbody.AddForce(Vector3.up * jumpPower * rigidbody.mass, ForceMode.Impulse);
		return true;
	}
}