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
	private bool isCharged = false;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public bool Charge()
	{
		audioSource.PlayOneShot(fireballCrarge);
		AudioManager.instance.PlayTribeVoc(fireballCrargeVoc);
		isCharged = true;

		
		return true;
	}

	public bool Shoot()
	{
		if (!isCharged) return false; // not shooting because 

		ObjectPooler.Instance.SpawnFromPool("Spell_Exploder", fireballSpawnpoint.position, fireballSpawnpoint.rotation);
		//Instantiate(fireballPrefab, fireballSpawnpoint.position, fireballSpawnpoint.rotation);

		audioSource.PlayOneShot(fireballThrow);
		AudioManager.instance.PlayDrum(fireballThrowDrum);
		AudioManager.instance.PlayTribeVoc(fireballThrowVoc);
		isCharged = false;

		return true;
	}
}
