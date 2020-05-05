using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExploderSpell : MonoBehaviour
{
	//
	// Other components
	#region Other components
	private AudioSource audioSource;
	#endregion

	//
	// Editor variables
	#region Editor variables
	public GameObject fireballPrefab = null;
	public Transform fireballSpawnpoint = null;
	public AudioClip fireballCrarge = null;
	public AudioClip fireballCrargeVoc = null;
	public AudioClip fireballThrow = null;
	public AudioClip fireballThrowDrum = null;
	public AudioClip fireballThrowVoc = null;
	#endregion

	//
	// Private variables
	#region Private variables
	private string chargeKey;
	private string fireKey;
	private bool hasShot = false;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------

	private void Awake()
	{
		chargeKey = "Fire2 P1";
		fireKey = "Fire1 P1";

		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (Input.GetButtonDown(chargeKey))
		{
			audioSource.PlayOneShot(fireballCrarge);
			AudioManager.instance.PlayTribeVoc(fireballCrargeVoc);
			hasShot = false;
		}

		if (Input.GetButtonDown(fireKey))
		{
			if (!Input.GetButton(chargeKey)) return;
			if (hasShot) return;

			Instantiate(fireballPrefab, fireballSpawnpoint.position, fireballSpawnpoint.rotation);

			audioSource.PlayOneShot(fireballThrow);
			AudioManager.instance.PlayDrum(fireballThrowDrum);
			AudioManager.instance.PlayTribeVoc(fireballThrowVoc);
			hasShot = true;
		}
	}
}
