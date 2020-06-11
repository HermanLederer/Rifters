using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkSpellObject : NetworkBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public float lifeTime = 1f; // The longevity of the spell
	#endregion

	//
	// Private variables
	#region Private variables
	private float deathTime = 0f;
	#endregion

	void OnEnable()
	{
		// Resetting the death time
		deathTime = Time.time + lifeTime;
	}

	void Update()
	{
		if (Time.time >= deathTime)
			NetworkServer.Destroy(gameObject);
	}
}