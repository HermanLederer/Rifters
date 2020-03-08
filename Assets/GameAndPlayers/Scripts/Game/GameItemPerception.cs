using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemPerception : MonoBehaviour
{
	// Editor variables

	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour methods
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
	// GameItemPerception methods
	//--------------------------
	public Player GetNearestPlayer()
	{
		Player nearestPlayer = GameManager.instance.players[0];
		float distance = Vector3.Distance(nearestPlayer.transform.position, transform.position);

		for (int i = 1; i < GameManager.instance.players.Count; ++i)
		{
			float dist = Vector3.Distance(GameManager.instance.players[i].transform.position, transform.position);
			if (dist < distance)
			{
				nearestPlayer = GameManager.instance.players[i];
				distance = dist;
			}
		}

		return nearestPlayer;
	}
}
