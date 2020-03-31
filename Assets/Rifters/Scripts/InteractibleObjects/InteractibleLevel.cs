using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleLevel : MonoBehaviour
{
    public enum TypeOfInteractableObject
    {
        PULL,
        SCALE,
        ROOTS,
        GEYSER
    }

    public TypeOfInteractableObject typeOfObject;

    public bool activated; //Comun

    public LayerMask playerLayer;

    protected Rigidbody rb; //Comun
    protected PlayerSpells myPlayer; //Comun

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); //Comun
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(playerLayer == (playerLayer.value | 1 << other.gameObject.layer))
        {
            Transform parent = other.transform.parent;
            PlayerSpells ps = parent.GetComponentInChildren<PlayerSpells>();
            if (ps != null)
            {
                Debug.Log("Entra Player: " + other.gameObject.name);
                ps.inRangeObjects.Add(this);
            }
        }
    }

    public virtual void OnTriggerStay(Collider other)
    {
        
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (playerLayer == (playerLayer.value | 1 << other.gameObject.layer))
        {
            Transform parent = other.transform.parent;
            PlayerSpells ps = parent.GetComponentInChildren<PlayerSpells>();
            if(ps != null)
            {
                Debug.Log("Sale Player: " + other.gameObject.name);
                ps.inRangeObjects.Remove(this);
            }
        }
    }

    public virtual void ActivateObject(PlayerSpells ps)
    {

    }
}
