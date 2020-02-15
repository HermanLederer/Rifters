using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGoalArea : MonoBehaviour
{
	// Editor variables
	[SerializeField] public MonoBehaviour gameItemObject { get; private set; }

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
			Debug.Log("A goal has been scored");
	}

	//--------------------------
	// GameGoalArea events
	//--------------------------
}
