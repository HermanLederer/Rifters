using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class IceWallController : NetworkBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public float lifeTime = 2f; // The longevity of the spell
	public LayerMask objectsLayerMask; // Objects we can freeze
	public float radius;
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

		// Sperecasting
		Vector3 sphereCastOrigin = transform.position;
		Collider[] colliders = Physics.OverlapSphere(sphereCastOrigin, radius, objectsLayerMask);

		foreach (Collider collider in colliders)
		{
			Rigidbody colliderRB;
			if (collider.TryGetComponent<Rigidbody>(out colliderRB))
			{
				colliderRB.velocity = Vector3.zero;
			}
		}
	}

	void Update()
	{
		if (Time.time >= deathTime)
			NetworkServer.Destroy(gameObject);
	}
}
