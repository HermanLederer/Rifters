using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ExploderSpellProjectile : NetworkBehaviour
{
	//
	// Other components
	#region Other components
	private Rigidbody rb = null;
	#endregion

	//
	// Editor variables
	#region Editor variables
	[SerializeField] private float speed = 5f;
	public AudioClip fireballExplode = null;
	public AudioClip fireballExplodeDrum = null;
	public AudioClip fireballExplodeVoc = null;
	public LayerMask explosionLayer = 0;
	public float lifeTime = 10f;
	#endregion

	//
	// Private variables
	#region Private variables
	private float deathTime = 0f;
	private bool isDead = false;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void OnEnable()
	{
		deathTime = Time.time + lifeTime;
		isDead = false;
		rb.velocity = Vector3.zero;
		rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
		gameObject.GetComponentsInChildren<Light>()[0].enabled = true;
	}

	public void OnDisable()
	{
		Explode();
	}

	private void Update()
	{
		if (Time.time >= deathTime)
		{
			Explode();
			NetworkServer.Destroy(gameObject);
		}
	}

	[Server]
	private void OnCollisionEnter(Collision collision)
	{
		transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
		NetworkServer.Destroy(gameObject);
	}

	//--------------------------
	// ExploderSpellProjectile methods
	//--------------------------
	public void Explode()
	{
		if (isDead) return;
		isDead = true;

		// SFX
		AudioManager.instance.PlayIn3D(fireballExplode, 1, transform.position, 5, 70);
		AudioManager.instance.PlayDrum(fireballExplodeDrum);
		AudioManager.instance.PlayTribeVoc(fireballExplodeVoc);

		// VFX
		VFXManager.instance.SpawnExplosionVFX(transform.position, transform.rotation);

		// Explosion force
		Collider[] colliders = Physics.OverlapSphere(transform.position, 18, explosionLayer);
		foreach (Collider other in colliders)
		{
			Rigidbody rb;
			if (other.TryGetComponent<Rigidbody>(out rb))
			{
				if (other.gameObject.GetInstanceID() == GetInstanceID()) continue;
				rb.AddExplosionForce(800, transform.position, 18, 0, ForceMode.Impulse);
			}
		}
	}
}
