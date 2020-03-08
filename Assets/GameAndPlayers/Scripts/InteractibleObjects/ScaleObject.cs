using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : InteractibleLevel
{
    // Editor variables
    public bool scaled; //Scale

    public float scaleSpeed = 3f; //Scale
    public float timeBeingScaled = 5f; //Scale

    public int maxSize = 8; //Scale

    public GameObject wallPs;

    // Public variables

    // Private variables
    private float timeToDownscale = -1; //Scale

    private int minSize; //Scale

    private Vector3 maxScale; //Scale
    private Vector3 startScale; //Scale

    //--------------------------
    // MonoBehaviour events
    //--------------------------

    private void Awake()
    {
        typeOfObject = TypeOfInteractableObject.SCALE;
        rb = GetComponent<Rigidbody>();

        startScale = transform.localScale; //Scale
        minSize = Mathf.RoundToInt(startScale.x); //Scale
        maxScale = new Vector3(startScale.x, startScale.y * maxSize, startScale.z); //Scale
        //maxScale = new Vector3(startScale.x * maxSize, startScale.y * maxSize, startScale.z * maxSize/2);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (!scaled)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, maxScale, scaleSpeed * Time.deltaTime);
                if (transform.localScale.y > maxSize - 0.2f)  //(transform.localScale.x > maxSize - 0.2f)
                {
                    transform.localScale = maxScale;
                    scaled = true;
                    timeToDownscale = timeBeingScaled;
                }
            }
            else
            {
                if (timeToDownscale > 0)
                {
                    timeToDownscale -= Time.deltaTime;
                }
                else
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                    if (transform.localScale.y < minSize + 0.2f)  //(transform.localScale.x < minSize + 0.2f)
                    {
                        transform.localScale = startScale;
                        activated = false;
                        scaled = false;
                    }
                }
            }
        }
    }

    public override void ActivateObject(PlayerSpells ps)
    {
        if (!activated)
        {
            ScaleTheObject();
        }
    }

    private void ScaleTheObject()
    {
        activated = true;
        GameObject ps = Instantiate(wallPs, transform.position, Quaternion.identity);
        Destroy(ps, 2f);
    }
}
