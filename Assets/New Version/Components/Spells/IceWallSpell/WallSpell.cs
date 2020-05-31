using UnityEngine;

public class WallSpell : Spell
{
	//
	// Editor variables
	[Header("Wall parameters")]
	public GameObject markerObjectPrefab = null; // Marker that will show how the wall will behave
	public LayerMask levelLayerMask = 1; // Level Layer

	//
	// Public variables
	[System.NonSerialized]
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

		marker = Instantiate(markerObjectPrefab);
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
	/// Raycasts player aim into the scene, enables the aiming marker and moves it to the raycast hit point.
	/// </summary>
	public void Aim()
	{
		// TODO: raycast from hand?
		RaycastHit hit;
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

		// Checking if the ray hits something
		if (Physics.Raycast(ray, out hit, 150f, levelLayerMask))
		{
			// TODO: use different markers depending on where the player is aiming
			//if (Vector3.Angle(Vector3.up, hit.normal) < 10)
			//{
				// flat marker for ground

				marker.transform.position = hit.point;
				// CHANGE: making it parallel to the ground instead
				Quaternion rotation = transform.rotation;
				marker.transform.rotation = rotation;
				marker.SetActive(true); // enable the marker
			//}
			//else
			//{
				// spherical market for walls and objects mid air?
			//}
		}
		else //--If the ray does not hit anything
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
		//Vector3 newRotation = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		Vector3 p = marker.transform.position;
		Quaternion r = marker.transform.rotation;
		player.CmdSpawnObject(1, p.x, p.y, p.z, r.x, r.y, r.z, r.w);

		player.ChangeSpellAlpha(TypeOfSpell.STOPSPELL, 0.5f);
		player.SetAnimTriggerSpell(animationTrigger);

		return true;
	}

	public override void OnFullyRecharged()
	{
		player.ChangeSpellAlpha(TypeOfSpell.STOPSPELL, 1f);
	}
}
