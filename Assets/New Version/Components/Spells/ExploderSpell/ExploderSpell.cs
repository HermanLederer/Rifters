using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExploderSpell : Spell
{
	//
	// Other components
	#region Editor variables
	[Header("Exploder parameters")]
	private AudioSource audioSource = null;
	#endregion

	//
	// Editor variables
	#region Editor variables
	[Header("Exploder parameters")]
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
	#region Editor variables
	[Header("Exploder parameters")]
	private GameObject lastFireball = null;
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	//--------------------------
	// Spell methods
	//--------------------------
	public override void OnFullyRecharged()
	{
		// SFX
		//audioSource.PlayOneShot(fireballCrarge);
		//AudioManager.instance.PlayTribeVoc(fireballCrargeVoc);
	}

	public override bool Trigger()
	{
		if (lastFireball != null && lastFireball.activeSelf) // explode the last fireball
		{
			lastFireball.GetComponent<ExploderSpellProjectile>().Explode();
		}
		else // shoot a fireball
		{
			if (!base.Trigger()) return false; // does cooldown

			// Get the direction vector
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 150f))
			{
				// projectile
				Vector3 direction = hit.point - fireballSpawnpoint.position;
				lastFireball = ObjectPooler.instance.SpawnFromPool("Spell_Exploder", fireballSpawnpoint.position, Quaternion.LookRotation(direction));

				// SFX
				audioSource.PlayOneShot(fireballThrow);
				AudioManager.instance.PlayDrum(fireballThrowDrum);
				AudioManager.instance.PlayTribeVoc(fireballThrowVoc);
			}
		}

		return true;
	}
}
