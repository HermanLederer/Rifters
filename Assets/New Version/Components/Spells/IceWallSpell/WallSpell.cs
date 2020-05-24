using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WallSpell : Spell
{
	//
	// Editor variables
	[Header("Wall parameters")]
	public GameObject markerPrefab; // Marker that will show how the wall will behave
	public LayerMask levelLayerMask; // Level Layer

	//
	// Public variables
	public bool isAiming = false;

	//
	// Private variables
	private GameObject marker;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	protected override void Start()
	{
		base.Start();

		marker = Instantiate(markerPrefab);
	}

	protected override void Update()
	{
		base.Update();

		if (isAiming) Aim();
		else Unaim();
	}

	//--------------------------
	// WallSpell methods
	//--------------------------

	/// <summary>
	/// Raycasts player aim into the scene and if the player is aiming at a flat surface enables the aiming marker and moves it to the raycast hit point.
	/// </summary>
	public void Aim()
	{
		RaycastHit hit;
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		// Checking if the ray hits the level and the normal of the level point is pointing up
		if (Physics.Raycast(ray, out hit, 50, levelLayerMask) && Vector3.Angle(hit.normal, Vector3.up) < 10)
		{
			marker.SetActive(true); // enable the marker
			marker.transform.position = hit.point;
			Quaternion rotation = Quaternion.LookRotation(transform.forward); // orientation of the player
			marker.transform.rotation = rotation;
		}
		else //--If the ray does not hit the level or the normal of the point is not pointing upwards
		{
			Unaim();
		}
	}

	/// <summary>
	/// Removes the aiming marker.
	/// </summary>
	public void Unaim()
	{
		marker.SetActive(false);
	}

	/// <summary>
	/// Checks if the spell is charged and creates a wall if it is.
	/// </summary>
	/// <returns>Whether the wall has been spawned.</returns>
	public override bool Trigger()
	{
		if (!base.Trigger()) return false; // does cooldown

		// Instantiate the wall
		Vector3 newRotation = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		ObjectPooler.instance.SpawnFromPool("Spell_Wall", marker.transform.position, Quaternion.Euler(newRotation));

		// Spawn VFX
		VFXManager.instance.SpawnFrostVFX(marker.transform.position, Quaternion.Euler(newRotation));

		return true;
	}
}
