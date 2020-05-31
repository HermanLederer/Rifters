using UnityEngine;

public class Spell : MonoBehaviour
{
	//
	// Editor variables
	#region Editor variables
	public float energyChargeRate = 1f;
	public int energyCharges = 1;
	public string animationTrigger;
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
		// Quite a lot of checks for what it is, but makes the UI implementation easy and allows for OnFullRecharge event to play sounds
		if (energy < 1f)
		{
			// Charging energy
			energy += Time.deltaTime * (energyChargeRate / energyCharges);
			if (energy > 1f)
			{
				energy = 1f;
				OnFullyRecharged();
			}
		}
	}

	//--------------------------
	// Spell methods
	//--------------------------
	public virtual void OnFullyRecharged() { }

	public bool HasCharge()
	{
		return energy >= 1f / energyCharges;
	}

	public virtual bool Trigger()
	{
		if (!HasCharge()) return false;

		energy -= 1f / energyCharges;
		return true;
	}
}
