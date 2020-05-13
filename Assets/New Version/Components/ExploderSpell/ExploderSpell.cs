using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderSpell : MonoBehaviour
{
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
	[Header("Audio")]
	public float volume = 1;
	public float minDistance = 0;
	public float maxDistance = 0;
	#endregion

	//
	// Private variables
	#region Private variables
	private bool isCharged = false;
	#endregion

	//--------------------------
	// ExploderSpell methods
	//--------------------------
	public bool Charge()
	{
		AudioManager.instance.PlayIn3D(fireballCrarge, volume, transform.position, minDistance, maxDistance);
		AudioManager.instance.PlayTribeVoc(fireballCrargeVoc);
		isCharged = true;
		
		return true;
	}

	public bool Shoot()
	{
		if (!isCharged) return false; // not shooting because 

		ObjectPooler.instance.SpawnFromPool("Spell_Exploder", fireballSpawnpoint.position, fireballSpawnpoint.rotation);

		AudioManager.instance.PlayIn3D(fireballThrow, volume, transform.position, minDistance, maxDistance);
		AudioManager.instance.PlayDrum(fireballThrowDrum);
		AudioManager.instance.PlayTribeVoc(fireballThrowVoc);
		isCharged = false;

		return true;
	}
}
