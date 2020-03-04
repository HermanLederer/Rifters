using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleLevel : MonoBehaviour
{
    public enum TypeOfInteractableObject{
        PULL,
        SCALE
    }

    [Header("Type of Object")]
    public TypeOfInteractableObject typeOfObject;

    public bool activated;
    public bool scaling;
    public float speed;

    private PlayerSpells myPlayer;

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
        if(typeOfObject == TypeOfInteractableObject.PULL)
        {
            if (activated)
            {
                transform.position = Vector3.MoveTowards(transform.position, myPlayer.ObjectHolder.transform.position, speed * Time.deltaTime);
            }
        }
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
            case TypeOfInteractableObject.PULL:
                if (!activated)
                {
                    PullObject(ps);
                }
                else
                {
                    ThrowObject(ps);
                }
                break;
            default:
                if (!activated)
                {
                    ScaleObject(true);
                }
                break;
        }
    }

    private void PullObject(PlayerSpells ps)
    {
        myPlayer = ps;

        rb.isKinematic = true;

        //transform.position = myPlayer.ObjectHolder.position;

        //transform.SetParent(myPlayer.ObjectHolder);

        activated = true;

        myPlayer.hasObject = true;
    }

    private void ThrowObject(PlayerSpells ps)
    {
        rb.isKinematic = false;

        //transform.SetParent(null);
        activated = false;

        rb.AddForce(ps.cam.transform.forward * ps.throwingForce);

        ps.hasObject = false;
    }

    private void ScaleObject(bool grow)
    {
        scaling = true;
    }
}
