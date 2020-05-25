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
	[Header("Projectile")]
	public GameObject exploderProjectilePrefab = null;
	public Transform exploderSpawnpoint = null;
	[Header("Sounds")]
	public AudioClip exploderCrarge = null;
	public AudioClip exploderCrargeVoc = null;
	public AudioClip exploderThrow = null;
	public AudioClip exploderThrowDrum = null;
	public AudioClip exploderThrowVoc = null;
	#endregion

	//
	// Private variables
	#region Editor variables
	[Header("Exploder parameters")]
	private GameObject lastFireball = null;
	private string poolName = "Exploder_Spell_Projectiles";
	#endregion

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		ObjectPooler.Instance.CreateNewPool(poolName, exploderProjectilePrefab, 8);
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
			lastFireball.GetComponent<ExploderSpellProjectile>().CmdExplode();
		}
		else // shoot a fireball
		{
			if (!base.Trigger()) return false; // does cooldown

			// Get the direction vector
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 150f))
			{
				// projectile
				Vector3 direction = hit.point - exploderSpawnpoint.position;
				lastFireball = ObjectPooler.Instance.SpawnFromPool(poolName, exploderSpawnpoint.position, Quaternion.LookRotation(direction));

				// SFX
				audioSource.PlayOneShot(exploderThrow);
				AudioManager.instance.PlayDrum(exploderThrowDrum);
				AudioManager.instance.PlayTribeVoc(exploderThrowVoc);
			}
		}

		return true;
	}
}
