using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public float cooldown = 1f;
	public int maxCharges = 1;
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
	private float nextChargeTime = 0;
	private int charges = 0;
	#endregion

	private void Start()
	{
		charges = maxCharges;
	}

	private void Update()
	{
		if (Time.time >= nextChargeTime && charges < maxCharges)
		{
			nextChargeTime = Time.time + cooldown;
			++charges;
		}
	}

	//--------------------------
	// ExploderSpell methods
	//--------------------------
	public bool hasCharge()
	{
		return charges > 0;
	}

	public virtual bool Trigger()
	{
		if (!hasCharge()) return false;

		--charges;
		return true;
	}
}
