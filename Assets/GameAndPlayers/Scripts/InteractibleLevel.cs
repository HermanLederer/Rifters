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
    public bool scaled;

    public float moveSpeed = 5f;
    public float scaleSpeed = 3f;
    public float timeBeingScaled = 5f;

    public int maxSize = 8;

    private float timeToDownscale = -1;

    private int minSize;

    private Rigidbody rb;
    private PlayerSpells myPlayer;
    private List<Transform> playersInRange = new List<Transform>();

    private Vector3 maxScale;
    private Vector3 startScale;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startScale = transform.localScale;
        minSize = Mathf.RoundToInt(startScale.x);
        //maxScale = new Vector3(startScale.x * maxSize, startScale.y * maxSize, startScale.z * maxSize/2);
        maxScale = new Vector3(startScale.x, startScale.y * maxSize, startScale.z);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (typeOfObject == TypeOfInteractableObject.PULL)
            {
                transform.position = Vector3.Lerp(transform.position, myPlayer.ObjectHolder.transform.position, moveSpeed * Time.deltaTime);
            }
            else if (typeOfObject == TypeOfInteractableObject.SCALE)
            {
                if (!scaled)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, maxScale, scaleSpeed * Time.deltaTime);
                    if(transform.localScale.y > maxSize - 0.2f)  //(transform.localScale.x > maxSize - 0.2f)
                    {
                        transform.localScale = maxScale;
                        scaled = true;
                        timeToDownscale = timeBeingScaled;
                    }
                }
                else
                {
                    if(timeToDownscale > 0)
                    {
                        timeToDownscale -= Time.deltaTime;
                    }
                    else
                    {
                        transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                        if(transform.localScale.y < minSize + 0.2f)  //(transform.localScale.x < minSize + 0.2f)
                        {
                            transform.localScale = startScale;
                            activated = false;
                            scaled = false;
                        }
                    }
                }
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
                    ScaleObject();
                }
                break;
        }
    }

    private void PullObject(PlayerSpells ps)
    {
        myPlayer = ps;

        rb.isKinematic = true;

        activated = true;

        myPlayer.hasObject = true;
    }

    private void ThrowObject(PlayerSpells ps)
    {
        rb.isKinematic = false;

        activated = false;

        rb.AddForce(ps.cam.transform.forward * ps.throwingForce);

        ps.hasObject = false;
    }

    private void ScaleObject()
    {
        activated = true;
    }
}
