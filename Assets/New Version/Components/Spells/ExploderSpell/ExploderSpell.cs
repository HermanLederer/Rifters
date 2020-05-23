using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderSpell : Spell
{
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

	//--------------------------
	// ExploderSpell methods
	//--------------------------
	/*public override void OnRecharged()
	{
		AudioManager.instance.PlayIn3D(fireballCrarge, volume, transform.position, minDistance, maxDistance);
		AudioManager.instance.PlayTribeVoc(fireballCrargeVoc);
	}*/

	public override bool Trigger()
	{
		if (!base.Trigger()) return false; // does cooldown

		// Get the direction vector
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f))
		{
			Vector3 direction = hit.point - fireballSpawnpoint.position;
			//Debug.DrawRay(fireballSpawnpoint.position, direction, Color.red, 1f);
			ObjectPooler.instance.SpawnFromPool("Spell_Exploder", fireballSpawnpoint.position, Quaternion.LookRotation(direction));

			AudioManager.instance.PlayIn3D(fireballThrow, volume, transform.position, minDistance, maxDistance);
			AudioManager.instance.PlayDrum(fireballThrowDrum);
			AudioManager.instance.PlayTribeVoc(fireballThrowVoc);
		}

		return true;
	}
}
