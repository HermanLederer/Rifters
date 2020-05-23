using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WallSpell : Spell
{
	[Header("Wall parameters")]
	public GameObject markerPrefab; // Marker that will show how the wall will behave
	public LayerMask levelLayerMask; // Level Layer

	private GameObject marker;

	private void Start()
	{
		marker = Instantiate(markerPrefab);
	}

	// mouse down
	public void Aim()
	{
		RaycastHit hit;
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		// Checking if the ray hits the level and the normal of the level point is pointing up
		if (Physics.Raycast(ray, out hit, 50, levelLayerMask) && Vector3.Angle(hit.normal, Vector3.up) < 10)
		{
			//marker.SetActive(true); //Enable the marker
			//rotation = Quaternion.LookRotation(transform.forward); //Orientation of the player
			//marker.transform.rotation = rotation;
			marker.transform.position = hit.point + hit.normal * 0.01f;

			Debug.DrawLine(transform.position, hit.point);
		}
		else //--If the ray does not hit the level or the normal of the point is not pointing upwards
		{
			//marker.SetActive(false);
		}
	}

	// mouse up
	public override bool Trigger()
	{
		if (!base.Trigger()) return false; // does cooldown

		//--Instantiate the wall--
		Vector3 newRotation = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		ObjectPooler.instance.SpawnFromPool("Spell_Wall", marker.transform.position, Quaternion.Euler(newRotation));
		
		//marker.SetActive(false);

		//NetworkServer.Spawn(iceWallInstance);
		return true;
	}
}
