using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlinkSpell : Spell
{
	//
	// Editor variables
	#region Editor variables
	[Header("Blink parameters")]
	public float blinkDuration;
	public float blinkDistance;

	public float raycastHeight = 1f;
	public float raycastRadius = 1f;

	public Ease easeType;
	public LayerMask layermask;

	public GameObject vfxPrefab;
	#endregion

	//
	// Private variables
	private Vector3 originalVelocity = Vector3.zero;
	private string vfxPoolName = "Pool_BlinkSpell";

	protected override void Start()
	{
		base.Start();

		ObjectPooler.Instance.CreateNewPool(vfxPoolName, vfxPrefab, 4);
	}

	//--------------------------
	// BlinkSpell methods
	//--------------------------
	public override bool Trigger()
	{
		if (!base.Trigger()) return false; // does cooldown

		//Vector3 direction = player.rigidbodyController.rigidbody.velocity.normalized;
		//if (Vector3.Angle(direction, transform.forward) < 46) direction = Camera.main.transform.forward;

		//// Original destination
		//Vector3 destination = transform.position + direction * blinkDistance;

		//// Looking for obstacles
		//Vector3 raycastOrigin1 = transform.position + transform.up * raycastRadius;
		//Vector3 raycastOrigin2 = transform.position + transform.up * (raycastHeight - raycastRadius);
		//RaycastHit wallHit;

		//// Looking for ground
		//if (Physics.CapsuleCast(raycastOrigin1, raycastOrigin2, raycastRadius, direction, out wallHit, blinkDistance, layermask))
		//{
		//	// changing the destination to the position at the wall
		//	destination = wallHit.point - direction * raycastRadius;
		//	RaycastHit groundHit;

		//	if (Physics.Raycast(destination, Vector3.down, out groundHit, raycastHeight, layermask))
		//	{
		//		// changing the destionation to the postion at the ground
		//		destination = groundHit.point;
		//	}
		//}

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, layermask))
		{
			// Twean
			originalVelocity = player.rigidbodyController.Rb.velocity;
			player.FreezeControls(blinkDuration);
			player.rigidbodyController.transform.DOMove(hit.point, blinkDuration).SetEase(easeType).OnComplete(RestoreVelocity);

			// UI
			player.ChangeSpellAlpha(TypeOfSpell.BLINK, .5f);

			// Animation
			player.SetAnimTriggerSpell(animationTrigger);

			// VFX
			GameObject vfx = ObjectPooler.Instance.SpawnFromPool(vfxPoolName, transform.position, transform.rotation, true);
			vfx.transform.parent = transform;
			return true;
		}

		return false;
	}

	private void RestoreVelocity()
	{
		player.rigidbodyController.Rb.velocity = originalVelocity;
	}

	public override void OnFullyRecharged()
	{
		player.ChangeSpellAlpha(TypeOfSpell.BLINK, 1f);
	}
}
