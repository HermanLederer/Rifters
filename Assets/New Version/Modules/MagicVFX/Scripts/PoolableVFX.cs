using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableVFX : MonoBehaviour
{
	[SerializeField] private List<ParticleSystem> particleSystems = null;

	protected void OnEnable()
	{
		// playing particle systems
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.Play();
		}
	}

	protected void OnDisable()
	{
		// stopping the particle systems
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.Stop();
		}
	}
}
