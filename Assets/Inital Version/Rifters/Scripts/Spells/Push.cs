using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
	public float radius = 3f;
	public float pushForce = 50f;
	public float tornadoTime = 3f;
	public float cooldown = 4f;
	public float animationSpellTime;
	public int angle = 30;

	public LayerMask dragonLayer;

	public GameObject tornadoVFX;

	private SpellsPlayer player;
	private string fireKey;
	private float remainingCd = -1;
	private void Awake()
	{
		player = GetComponentInParent<SpellsPlayer>();

		fireKey = "Fire1 P1";
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		#region drawing
		Vector3 reference = transform.forward * radius;
		Vector3 up = Quaternion.AngleAxis(angle, transform.right) * reference;
		Vector3 down = Quaternion.AngleAxis(-angle, transform.right) * reference;
		Vector3 left = Quaternion.AngleAxis(angle, transform.up) * reference;
		Vector3 right = Quaternion.AngleAxis(-angle, transform.up) * reference;

		Debug.DrawLine(transform.position, transform.position + up, Color.green);
		Debug.DrawLine(transform.position, transform.position + down, Color.green);
		Debug.DrawLine(transform.position, transform.position + left, Color.green);
		Debug.DrawLine(transform.position, transform.position + right, Color.green);
		#endregion
		if(remainingCd > 0)
		{
			remainingCd -= Time.deltaTime;
		}
		else
		{
			if (Input.GetButton(fireKey))
			{
				//player.SetAnimTriggerSpell("Push");

				Quaternion look = Quaternion.LookRotation(transform.forward, transform.up);

				GameObject tornado = Instantiate(tornadoVFX, transform.position, look);
				tornado.transform.parent = transform;

				Destroy(tornado, tornadoTime);
				remainingCd = cooldown;
				PushObjects();
			}
		}
		
	}

	private void PushObjects()
	{
		Collider[] pushableObjects = Physics.OverlapSphere(transform.position, radius, dragonLayer);

		int contador = 0;

		for (int i = 0; i < pushableObjects.Length; i++)
		{
			if (Vector3.Angle(transform.forward, pushableObjects[i].transform.position - transform.position) > angle)
			{
				continue;
			}

			contador += 1;

			Rigidbody rb = pushableObjects[i].GetComponent<Rigidbody>();

			if (rb != null)
			{
				rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
			}
		}

		Debug.Log("Numero de objetos en el cono: " + contador);
	}
}
