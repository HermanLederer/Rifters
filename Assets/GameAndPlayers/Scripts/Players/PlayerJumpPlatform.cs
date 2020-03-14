using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpPlatform : MonoBehaviour
{
	// Other variables

	// Editor variables
	public float radius;
	public float shootPower;
	public float shootMaxSpeed;
	public float shootAcceleration;
	public float shootAmplitude;
	public float stayDuration;
	public GameObject platform;

	// Public variables

	// Private variables
	private float speed;
	private float targetHeight;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	//--------------------------
	// PlayerJumpPlatform events
	//--------------------------
	public void Grab()
	{
		foreach (Collider collider in Physics.OverlapSphere(transform.position, radius))
		{
			JumpPlatformInteractable interactable = collider.gameObject.GetComponent<JumpPlatformInteractable>();

			if (interactable != null)
			{
				interactable.ChangeParent(transform);
			}
		}
	}

	public void Shoot(Vector3 position)
	{
		gameObject.SetActive(true);
		
		transform.position = position;
		transform.position += Vector3.down * 0.1f;
		StartCoroutine(ShootCorutine());
	}

	private IEnumerator ShootCorutine()
	{
		// Grab objects
		Grab();

		// Shoot
		speed = 0;
		targetHeight = transform.position.y + 0.1f + shootAmplitude;

		while (transform.position.y < targetHeight)
		{
			if (speed < shootMaxSpeed) speed += shootAcceleration;
			transform.position += Vector3.up * speed * Time.deltaTime;

			yield return null;
		}
		
		// Launch objects up
		foreach (JumpPlatformInteractable interactable in GetComponentsInChildren<JumpPlatformInteractable>())
		{
			interactable.Launch(Vector3.up * shootPower);
		}

		// Stay
		yield return new WaitForSeconds(stayDuration);

		// Decay
		speed = 0;
		targetHeight = transform.position.y - shootAmplitude;

		while (transform.position.y > targetHeight)
		{
			speed += shootAcceleration;
			transform.position += Vector3.down * speed * Time.deltaTime;

			yield return null;
		}
		Destroy(gameObject);
	}
}
