using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpells : NetworkBehaviour
{
	// Other variables

	// Editor variables

	// Public variables
	public float minAngle = 20;
	public float throwingForce = 10;
	public float raycastDistance = 6;
	public float spellAnimationTime;
	public float cooldown;

	public Camera cam;

	public bool hasObject = false;

	//public GameObject SpellPanel;
	public Transform ObjectHolder;
	public LayerMask InteractibleLayer;

	//[HideInInspector]
	public List<InteractibleLevel> inRangeObjects = new List<InteractibleLevel>();

	// Private variables
	private InteractibleLevel activableObject;
	private Player player;
	private string spellKey;
	private float remainingCd = -1f;

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		player = GetComponentInParent<Player>();

		if (player.isPlayer1)
		{
			spellKey = "Enviroment Spells P1";
		}
		else
		{
			spellKey = "Enviroment Spells P2";
		}
	}

	void FixedUpdate()
	{
		if (!hasObject)
		{
			DetectInteractibleObjects();
		}

		if (remainingCd > 0)
		{
			remainingCd -= Time.deltaTime;
		}
		else
		{
			if (Input.GetButton(spellKey))
			{
				if (activableObject != null)
				{
					player.SetAnimTriggerSpell("EnviromentSpell", spellAnimationTime);
					activableObject.ActivateObject(this);
					remainingCd = cooldown;
				}
			}
		}

	}

	//--------------------------
	// PlayerSpells methods
	//--------------------------
	private void DetectInteractibleObjects()
	{
		activableObject = null;

		//If the player is looking that object
		for (int i = 0; i < inRangeObjects.Count; i++)
		{
			Vector3 dir = (inRangeObjects[i].transform.position - cam.transform.position).normalized;
			Debug.DrawLine(inRangeObjects[i].transform.position, cam.transform.position);

			float angle = Vector3.Angle(dir, cam.transform.forward);

			bool lookingAt = false;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, raycastDistance, InteractibleLayer))
			{
				lookingAt = true;
			}
			if (angle < minAngle || lookingAt)
			{
				activableObject = inRangeObjects[i];
				break;
			}
		}
		/*

        if (activableObject != null)
        {
            if (activableObject.typeOfObject == InteractibleLevel.TypeOfInteractableObject.PULL || !activableObject.activated)
            {
                SpellPanel.SetActive(true);
            }
            else
            {
                SpellPanel.SetActive(false);
            }

        }
        else
        {
            SpellPanel.SetActive(false);
        }*/
	}
}
