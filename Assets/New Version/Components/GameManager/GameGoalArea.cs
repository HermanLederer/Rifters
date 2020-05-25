using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGoalArea : MonoBehaviour
{
	//
	// Editor variables
	//[SerializeField] private GameTeam team = GameTeam.Team1;

	//
	// Public variables

	//
	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void OnTriggerEnter(Collider other)
	{
		/*if (other.GetComponent<GameItemBehaviour>() != null)
			GameManager.instance.Score(team);*/
	}
}
