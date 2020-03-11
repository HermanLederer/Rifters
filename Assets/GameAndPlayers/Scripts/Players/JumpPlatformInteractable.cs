using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatformInteractable : MonoBehaviour
{
	// OtherComponents
	private Rigidbody _rigidbody;

	// Public variables
	[HideInInspector] public Transform originalParent;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		TryGetComponent<Rigidbody>(out _rigidbody);
	}

	//--------------------------
	// JumpPlatformInteractable events
	//--------------------------
	public void ChangeParent(Transform parent)
	{
		originalParent = transform.parent;
		transform.parent = parent;
	}

	public void Launch(Vector3 launchVelocity)
	{
		transform.parent = originalParent;

		if (_rigidbody != null) { _rigidbody.AddForce(launchVelocity); }
	}
}
