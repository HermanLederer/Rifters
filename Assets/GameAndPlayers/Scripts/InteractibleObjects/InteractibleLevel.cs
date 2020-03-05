using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleLevel : MonoBehaviour
{
    public enum TypeOfInteractableObject
    {
        PULL,
        SCALE
    }

    public TypeOfInteractableObject typeOfObject;

    public bool activated; //Comun

    protected Rigidbody rb; //Comun
    protected PlayerSpells myPlayer; //Comun

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); //Comun
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            other.GetComponent<PlayerSpells>().inRangeObjects.Add(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<PlayerSpells>().inRangeObjects.Remove(this);
        }
    }

    public virtual void ActivateObject(PlayerSpells ps)
    {

    }
}
