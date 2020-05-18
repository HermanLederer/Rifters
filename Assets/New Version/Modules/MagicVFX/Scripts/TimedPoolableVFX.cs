using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPoolableVFX : PoolableVFX
{
	[SerializeField] private float lifeTime = 2f;
	private float deathTime = 0;

	void Update()
	{
		if (Time.time >= deathTime)
		{
			gameObject.SetActive(false);
		}
	}

	new protected void OnEnable()
	{
		base.OnEnable();
		deathTime = Time.time + lifeTime;
	}
}
