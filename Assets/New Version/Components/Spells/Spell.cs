using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public float energyChargeRate = 1f;
	public int energyCharges = 1;
	[Header("Audio")]
	public float volume = 1;
	public float minDistance = 0;
	public float maxDistance = 0;
	[Header("References")]
	public Player player;
	#endregion

	//
	// Private variables
	#region Private variables
	private float energy = 0f;
	#endregion

	protected virtual void Start()
	{
		energy = 1;
	}

	protected virtual void Update()
	{
		// Charging energy
		energy += Time.deltaTime * (energyChargeRate / energyCharges);
		if (energy > 1f) energy = 1f;
	}

	//--------------------------
	// ExploderSpell methods
	//--------------------------
	public bool hasCharge()
	{
		return energy >= 1 / energyCharges;
	}

	public virtual bool Trigger()
	{
		if (!hasCharge()) return false;

		energy -= 1 / energyCharges;
		return true;
	}
}
