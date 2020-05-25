using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ExploderSpellProjectile : MonoBehaviour, IPooledObject
{
	//
	// Other components
	#region Other components
	new private Rigidbody rigidbody = null;
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
		rigidbody = GetComponent<Rigidbody>();
	}

	public void OnObjectSpawn()
	{
		deathTime = Time.time + lifeTime;
		isDead = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
		gameObject.GetComponentsInChildren<Light>()[0].enabled = true;
	}

	private void Update()
	{
		if (Time.time >= deathTime) Explode();
	}

	private void OnCollisionEnter(Collision collision)
	{
		transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
		Explode();
		
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
		Collider[] colliders = Physics.OverlapSphere(transform.position, 9, explosionLayer);
		foreach (Collider other in colliders)
		{
			Rigidbody rb;
			if (other.TryGetComponent<Rigidbody>(out rb))
			{
				if (other.gameObject.GetInstanceID() == GetInstanceID()) continue;
				rb.AddExplosionForce(700, transform.position, 9, 0, ForceMode.Impulse);
			}
		}

		gameObject.SetActive(false);
	}
}
