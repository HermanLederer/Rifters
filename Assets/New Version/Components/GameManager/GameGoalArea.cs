using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGoalArea : NetworkBehaviour
{
	// Editor variables
	[SerializeField] private GameTeam team = GameTeam.Team1;

	private NetworkManagerRifter room;

	private NetworkManagerRifter Room
	{
		get
		{
			if (room != null) { return room; }

			return room = NetworkManager.singleton as NetworkManagerRifter;
		}
	}
	//
	// Public variables

	//
	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Dragon")
			Room.Score(team);
	}
}
