using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceWallController : MonoBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public float lifeTime = 2f; // Time the wall will be placed if does not hit anything
	public LayerMask objectsLayerMask; // Objects the wall can freeze
	#endregion

	//
	// Private variables
	#region Private variables
	private float deathTime = 0f;
	#endregion

	void OnEnable()
	{
		//--Parameters for the box cast--
		//Vector3 boxCastPosition = transform.position + Vector3.up * (wallHeight / 2); //Box position
		//Quaternion dir = Quaternion.LookRotation(transform.forward); //Box orientation

		//--Box Cast--
		//Collider[] colls = Physics.OverlapBox(boxCastPosition, new Vector3(4.5f, 4f, 1f), dir, objectsLayerMask);

		//--If the cast hits something--
		//if (colls.Length > 0)
		//{
		//somethingIsTrapped = true;
		//	_lifeTime = .5f;
		//}
		//--If the cast does not hit anything--
		//else
		{
			deathTime = Time.time + lifeTime;
		}
	}

	void Update()
	{
		if (Time.time >= deathTime)
			gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(1);

		// SFX
		//AudioManager.instance.PlayIn3D(fireballExplode, 1, transform.position, 5, 70);
		//AudioManager.instance.PlayDrum(fireballExplodeDrum);
		//AudioManager.instance.PlayTribeVoc(fireballExplodeVoc);

		// VFX
		//VFXManager.instance.SpawnExplosionVFX(transform.position, Quaternion.FromToRotation(transform.forward, collision.contacts[0].normal));

		gameObject.SetActive(false);
	}
}
