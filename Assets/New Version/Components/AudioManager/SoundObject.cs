using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : NetworkBehaviour
{
	private AudioSource audioSource;
	private float deathTime = 10f;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		deathTime += Time.time;
	}

	public void OnEnable()
	{
		deathTime += Time.time;
		StartCoroutine(UpdateDeathTime());
	}

	private IEnumerator UpdateDeathTime()
	{
		yield return null;
		deathTime = Time.time + audioSource.clip.length;
	}

	void Update()
	{
		if (Time.time >= deathTime)
		{
			gameObject.SetActive(false);
		}
	}
}
