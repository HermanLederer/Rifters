using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatformInteractable : MonoBehaviour
{
	// OtherComponents
	private Rigidbody _rigidbody;
	private PlayerMovement _playerMovement;

	// Public variables
	[HideInInspector] public Transform originalParent;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		TryGetComponent<Rigidbody>(out _rigidbody);
		TryGetComponent<PlayerMovement>(out _playerMovement);
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

		if (_playerMovement != null) { _playerMovement.Velocity += launchVelocity;}
		else if (_rigidbody != null) { _rigidbody.velocity += launchVelocity; }
	}
}
