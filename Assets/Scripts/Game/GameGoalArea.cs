using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGoalArea : MonoBehaviour
{
	// Editor variables
	[SerializeField] private GameTeam team;

	// Public variables

	// Private variables

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

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<GameItem>() != null)
			GameManager.instance.Score(team);
	}

	//--------------------------
	// GameGoalArea events
	//--------------------------
}
