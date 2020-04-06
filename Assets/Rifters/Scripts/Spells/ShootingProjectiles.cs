using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingProjectiles : MonoBehaviour
{
	public float shootingForce = 30f;
	public float timeBtwProjectiles = .1f;

	public int projectileCount = 6;

	public bool shooting;

	public GameObject projectilePrefab;

	public float offsetX = 1;
	public float offsetY = 1;

	private Player player;
	private string fireKey;

	private void Awake()
	{
		player = GetComponentInParent<Player>();

		if (player.isPlayer1)
		{
			fireKey = "Fire2 P1";
		}
		else
		{
			fireKey = "Fire2 P2";
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!shooting)
		{
			if (Input.GetButton(fireKey))
			{
				shooting = true;
				StartCoroutine(ShootProjectiles());
			}
		}
	}

	IEnumerator ShootProjectiles()
	{
		Vector3 direction = (transform.right * offsetX + transform.up * offsetY);

		float angle = Vector3.Angle(transform.up, direction);
		float rotationdegrees = (angle * 2) / (projectileCount - 1);

		for (int i = 0; i < projectileCount; i++)
		{
			Debug.DrawRay(transform.position, direction, Color.magenta, 0.5f);
			Quaternion rotation = Quaternion.LookRotation(direction);

			GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, rotation);
			projectile.GetComponent<Rigidbody>().AddForce(direction * shootingForce);

			direction = Quaternion.AngleAxis(rotationdegrees, transform.forward) * direction;

			yield return new WaitForSeconds(timeBtwProjectiles);
		}

		shooting = false;
		yield return null;
	}
}
