using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class temp : MonoBehaviour
{
	public Transform from;
	public Transform to;
	public float cooldown = 1f;
	public float duration = 0.5f;
	public ParticleSystem ps;

	private float nextBlinkTime = 0f;

    void Start()
	{
		transform.position = from.position;
    }

    void Update()
    {
		if (Time.time >= nextBlinkTime)
		{
			nextBlinkTime = Time.time + cooldown + duration;
			StartCoroutine(Blink());
		}
    }

	private IEnumerator Blink()
	{
		// Spawn fx
		yield return new WaitForSeconds(0.3f);

		//Vector3 offset = from.position - to.position;

		transform.position = from.position;
		transform.DOMove(to.position, duration).SetEase(Ease.InOutFlash);

		yield return new WaitForSeconds(duration);
		// Spawn fx
		//VFXManager.instance.SpawnExplosionVFX(to.position, to.rotation);
	}
}
