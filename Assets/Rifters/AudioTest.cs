using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
	[SerializeField] private AudioClip JumpOnGrass;
	[SerializeField] private AudioClip JumpOnRocks;
	[SerializeField] private AudioClip MoveOnGrass;
	[SerializeField] private AudioClip MoveOnRocks;
	[SerializeField] private AudioClip Environment;



	private bool Jumped = true;

	private PlayerPhysicsWalker playerMovement;
	private Player player;


	private void Awake()
	{
		playerMovement = FindObjectOfType<PlayerPhysicsWalker>();
	}
	private void Start()
	{
		AudioManager.Instance.PlayBackgroundMusic(Environment, 0.5f);
	}
	private void FixedUpdate()
	{
		if (playerMovement.isGrounded)
		{
			Jumped = false;
		}

		if (Input.GetKeyDown(KeyCode.Space))

			if (Jumped == false)
			{
				AudioManager.Instance.PlayJump(JumpOnGrass, 1);
				Jumped = true;
			}

		if (Input.GetKeyDown(KeyCode.W) && playerMovement.isGrounded)
		{
			AudioManager.Instance.PlayMusic(MoveOnGrass);
		}

		Vector3 velocity = playerMovement.rigidbody.velocity;
		Vector3 localVelocity = player.playerOrigin.InverseTransformDirection(velocity);
		if (Input.GetKeyUp(KeyCode.W) || Jumped == true)// || localVelocity.magnitude == 0)
			AudioManager.Instance.StopMusic(MoveOnGrass);
	}
}
