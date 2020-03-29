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

	private PlayerMovement playerMovement;

	private void Awake()
	{
		playerMovement = FindObjectOfType<PlayerMovement>();
	}
	private void Start()
	{
		AudioManager.Instance.PlayBackgroundMusic(Environment, 0.5f);
	}
	private void Update()
	{
		//if (playerMovement.isGrounded)
		//{
		//	Jumped = false;
		//}
	
		if (Input.GetKeyDown(KeyCode.Space))
			if (Jumped == false)
			{
				AudioManager.Instance.PlayJump(JumpOnGrass, 1);
				Jumped = true;
			}

		if (Input.GetKeyDown(KeyCode.W))
			AudioManager.Instance.PlayMusic(MoveOnGrass);

		if (Input.GetKeyUp(KeyCode.W))
			AudioManager.Instance.StopMusic(MoveOnGrass);
	}
}
