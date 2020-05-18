﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class MagicVFXExplosionLight : MonoBehaviour
{
	//
	// Other components
	#region Other components
	new private Light light = null;
	#endregion

	//
	// Editor variables
	#region Editor variables
	[SerializeField] private float maxIntensity = 100f;
	[SerializeField] private float minLifeTime = 0.8f;
	[SerializeField] private float maxLifeTime = 1f;
	#endregion

	//
	// Private variables
	#region Private variables
	private float bornTime = 0f;
	private float lifeTime = 0f;
	#endregion

	//--------------------------
	// MonoBehaviourPunCallbacks events
	//--------------------------
	void Awake()
	{
		light = GetComponent<Light>();
	}

	private void OnEnable()
	{
		bornTime = Time.time;
		lifeTime = Random.Range(minLifeTime, maxLifeTime);
		light.intensity = maxIntensity;
	}

    void Update()
    {
		if ((Time.time - bornTime) > lifeTime) light.intensity = 0;
		else light.intensity = Mathf.Lerp(maxIntensity, 0, (Time.time - bornTime) / lifeTime);
	}
}