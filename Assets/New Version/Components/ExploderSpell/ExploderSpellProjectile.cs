using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ExploderSpellProjectile : MonoBehaviour
{
	//
	// Other components
	#region Other components
	private AudioSource audioSource = null;
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
	#endregion

	//
	// Private variables
	#region Private variables
	private bool isDead = false;
	#endregion

	//--------------------------
	// MonoBehaviourPunCallbacks events
	//--------------------------
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	void Start()
    {
		rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isDead) return;

		isDead = true;
		audioSource.PlayOneShot(fireballExplode);
		AudioManager.instance.PlayDrum(fireballExplodeDrum);
		AudioManager.instance.PlayTribeVoc(fireballExplodeVoc);

		Collider[] colliders = Physics.OverlapSphere(transform.position, 9, explosionLayer);
		foreach (Collider other in colliders)
		{
			Rigidbody rb;
			if (other.TryGetComponent<Rigidbody>(out rb))
			{
				if (other.gameObject.GetInstanceID() == GetInstanceID()) continue;
				rb.AddExplosionForce(700, transform.position, 5, 0, ForceMode.Impulse);
			}
		}

		gameObject.GetComponentsInChildren<Light>()[0].enabled = false;
		// TODO: Spawn fx
		//Destroy(gameObject);
	}
}
