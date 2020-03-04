using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLevel : MonoBehaviour
{
    public enum TypeOfInteractableObject{
        PULLABLE,
        SCALABLE
    }

    [Header("Type of Object")]
    public TypeOfInteractableObject typeOfObject;

    public bool activated;

    private List<Transform> playersInRange = new List<Transform>();

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void ActivateObject(PlayerSpells ps)
    {
        switch (typeOfObject)
        {
            case TypeOfInteractableObject.PULLABLE:
                if (!activated)
                {
                    PullObject(ps);
                }
                break;
            default:
                Debug.Log("Scale");
                break;
        }
    }

    private void PullObject(PlayerSpells ps)
    {
        rb.isKinematic = true;

        transform.position = ps.ObjectHolder.position;

        transform.SetParent(ps.ObjectHolder);

        ps.hasObject = true;
    }

    public void ThrowObject(PlayerSpells ps)
    {
        rb.isKinematic = false;

        transform.SetParent(null);

        rb.AddForce(ps.cam.transform.forward * ps.throwingForce);

        ps.hasObject = false;
    }
}
