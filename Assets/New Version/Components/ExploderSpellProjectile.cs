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
		//Destroy(gameObject);
	}
}
