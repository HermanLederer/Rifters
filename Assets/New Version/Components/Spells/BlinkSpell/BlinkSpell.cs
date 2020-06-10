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
	public float blinkDuration = 0.3f;
	public float blinkDistance = 30f;
	public float postBlinkVelocity = 30f;

	public float raycastHeight = 1f;
	public float raycastRadius = 1f;

	public Ease easeType;
	public LayerMask layermask;

	public GameObject vfxPrefab;
	#endregion

	//
	// Public variables
	public Vector2 direction = Vector2.zero;

	//
	// Private variables
	private Vector3 originalVelocity = Vector3.zero;

	protected override void Start()
	{
		base.Start();
	}

	//--------------------------
	// BlinkSpell methods
	//--------------------------
	public override bool Trigger()
	{
		if (!base.Trigger()) return false; // does cooldown

		// Getting direction from this.direction that stores player input
		Vector3 direction = Vector3.zero;
		if (this.direction.y > 0f) direction += transform.forward;
		else if (this.direction.y < 0f) direction -= transform.forward;
		if (this.direction.x > 0f) direction += transform.right;
		else if (this.direction.x < 0f) direction -= transform.right;
		direction = direction.normalized;
		if (direction.magnitude <= 0) direction = transform.forward;

		// Preferred destination
		Vector3 destination = transform.position + direction * blinkDistance;

		// Looking for obstacles and changing the destination if needed
		Vector3 raycastOrigin1 = transform.position + transform.up * raycastRadius;
		Vector3 raycastOrigin2 = transform.position + transform.up * (raycastHeight - raycastRadius);

		// Looking for ground and putting the destination there if it is close enough
		if (Physics.CapsuleCast(raycastOrigin1, raycastOrigin2, raycastRadius, direction, out RaycastHit wallHit, blinkDistance, layermask))
		{
			// changing the destination to the position at the wall
			destination = wallHit.point - direction * raycastRadius;

			if (Physics.Raycast(destination, Vector3.down, out RaycastHit groundHit, raycastHeight + 1, layermask))
			{
				// changing the destionation to the postion at the ground
				destination = groundHit.point;
			}
		}

		// Twean
		originalVelocity = direction * postBlinkVelocity;
		player.FreezeControls(blinkDuration);
		player.rigidbodyController.transform.DOMove(destination, blinkDuration).SetEase(easeType).OnComplete(RestoreVelocity);

		// UI
		player.ChangeSpellAlpha(TypeOfSpell.BLINK, .5f);

		// Animation
		player.SetAnimTriggerSpell(animationTrigger);

		// Spell object
		Vector3 p = transform.localPosition;
		Quaternion r = transform.localRotation;
		player.CmdSpawnChildObject(2, p.x, p.y, p.z, r.x, r.y, r.z, r.w);
		return true;
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
