using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsObject : InteractibleLevel
{
    public float livingTime = 8f;
    public float holdedObjectSpeed = 5f;

    public Transform holdingPosition;
    public ParticleSystem RootPs;

    public LayerMask dragonLayer;

    private GameObject holdedObject;
    private bool holding;

    private float lifeTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
            }
            else
            {
                if (holdedObject != null)
                {
                    holdedObject.GetComponent<Rigidbody>().useGravity = true;
                }

                lifeTime = 0;
                activated = false;
                holding = false;
                holdedObject = null;
                RootPs.Stop();
            }

            if (holding)
            {
                holdedObject.transform.position = Vector3.MoveTowards(holdedObject.transform.position, holdingPosition.position, holdedObjectSpeed * Time.deltaTime);
            }
        }
    }

    public override void ActivateObject(PlayerSpells ps)
    {
        if (!activated)
        {
            activated = true;
            lifeTime = livingTime;
            RootPs.Play();
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (dragonLayer == (dragonLayer.value | 1 << other.gameObject.layer) && activated)
        {
            SetHoldedObject(other.gameObject);
        }
    }*/

    private void SetHoldedObject(GameObject other)
    {
        holdedObject = other;

        GameItemBehaviour dragon = holdedObject.GetComponent<GameItemBehaviour>();

        dragon.BecomeBall();
        dragon.SetNextDragonTime(lifeTime + 1);

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            holding = true;
        }
    }
}
