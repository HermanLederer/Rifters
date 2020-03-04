using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpells : MonoBehaviour
{
    // Other variables

    // Editor variables

    // Public variables
    public float minAngle = 20;
    public float throwingForce = 10;

    public Camera cam;

    public bool hasObject = false;

    public GameObject SpellPanel;
    public Transform ObjectHolder;

    //[HideInInspector]
    public List<InteractibleLevel> inRangeObjects = new List<InteractibleLevel>();

    // Private variables
    public InteractibleLevel activableObject;

    //--------------------------
    // MonoBehaviour events
    //--------------------------
    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!hasObject)
        {
            activableObject = null;

            //If the player is looking that object
            for (int i = 0; i < inRangeObjects.Count; i++)
            {
                Vector3 dir = (inRangeObjects[i].transform.position - cam.transform.position).normalized;
                Debug.DrawLine(inRangeObjects[i].transform.position, cam.transform.position);

                float angle = Vector3.Angle(dir, cam.transform.forward);
                if (angle < minAngle)
                {
                    activableObject = inRangeObjects[i];
                    break;
                }
            }

            if (activableObject != null)
            {
                SpellPanel.SetActive(true);
            }
            else
            {
                SpellPanel.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(activableObject != null)
            {
                activableObject.ActivateObject(this);
            }
        }
    }

    //--------------------------
    // PlayerSpells events
    //--------------------------
}
