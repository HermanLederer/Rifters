using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTester : MonoBehaviour
{
	[SerializeField] private float lifeTime = 2f;
	[SerializeField] private List<ParticleSystem> particleSystems = null;
	[SerializeField] private MagicVFXExplosionLight explosionLight = null;

	private float deathTime = 0;

	private void OnEnable()
	{
		deathTime = Time.time + lifeTime;

		explosionLight.enabled = true;
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.Play();
		}
	}

	void Update()
	{
		// Stopping the particle systems and killing the object
		if (Time.time >= deathTime)
		{
			gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		explosionLight.enabled = false;
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.Stop();
		}
	}
}
