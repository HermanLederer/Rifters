using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishArea : MonoBehaviour
{
	// Editor variables

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
		if (other.GetComponent<Player>() != null)
			GameManager.instance.Win();
	}

	//--------------------------
	// GameFinishArea events
	//--------------------------
}
