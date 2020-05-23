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
	#endregion

	public Ease easeType;
	public LayerMask layermask;

	private Vector3 originalVelocity = Vector3.zero;

	//--------------------------
	// BlinkSpell methods
	//--------------------------
	public override bool Trigger()
	{
		if (!base.Trigger()) return false; // does cooldown

		Vector3 direction = player.rigidbodyController.rigidbody.velocity.normalized;
		if (Vector3.Angle(direction, transform.forward) < 46) direction = Camera.main.transform.forward;

		// Original destination
		Vector3 destination = transform.position + direction * blinkDistance;

		// Looking for obstacles
		Vector3 raycastOrigin1 = transform.position + transform.up * raycastRadius;
		Vector3 raycastOrigin2 = transform.position + transform.up * (raycastHeight - raycastRadius);
		RaycastHit wallHit;

		// Looking for ground
		if (Physics.CapsuleCast(raycastOrigin1, raycastOrigin2, raycastRadius, direction, out wallHit, blinkDistance, layermask))
		{
			// changing the destination to the position at the wall
			destination = wallHit.point - direction * raycastRadius;
			RaycastHit groundHit;

			if (Physics.Raycast(destination, Vector3.down, out groundHit, raycastHeight, layermask))
			{
				// changing the destionation to the postion at the ground
				destination = groundHit.point;
			}
		}

		// Twean
		originalVelocity = player.rigidbodyController.rigidbody.velocity;
		player.FreezeControls(blinkDuration);
		player.rigidbodyController.transform.DOMove(destination, blinkDuration).SetEase(easeType).OnComplete(RestoreVelocity);
		return true;
	}

	private void RestoreVelocity()
	{
		player.rigidbodyController.rigidbody.velocity = originalVelocity;
	}
}
