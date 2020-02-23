using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatformInteractable : MonoBehaviour
{
	// OtherComponents
	private Rigidbody _rigidbody;
	private PlayerMovement _playerMovement;

	// Editor variables

	// Public variables
	[HideInInspector] public Transform originalParent;

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		TryGetComponent<Rigidbody>(out _rigidbody);
		TryGetComponent<PlayerMovement>(out _playerMovement);

		originalParent = transform.parent;
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	//--------------------------
	// JumpPlatformInteractable events
	//--------------------------
	public void Launch(Vector3 launchVelocity)
	{
		// Uncomment when Grab() is implemented
		//transform.parent = originalParent;

		if (_playerMovement != null) { _playerMovement.velocity = launchVelocity;}
		else if (_rigidbody != null) { _rigidbody.velocity = launchVelocity; }
	}
}
