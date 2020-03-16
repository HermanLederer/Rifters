using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour
{
    public float radius;

    public Transform holdingPosition;

    public float HoldingTime = 4f;
    public float livingTime = 8f;
    public float holdedObjectSpeed = 5f;

    public LayerMask dragonLayer;

    private float timeHolding;
    private bool holding;
    private Animator holdingAnimator;
    private float lifeTime;

    private GameObject holdedObject;

    // Start is called before the first frame update
    void Awake()
    {
        holdingAnimator = holdingPosition.GetComponent<Animator>();
        lifeTime = livingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

        if (holding)
        {
            holdedObject.transform.position = Vector3.MoveTowards(holdedObject.transform.position, holdingPosition.position, holdedObjectSpeed * Time.deltaTime);
            /*if(timeHolding > 0)
            {
                timeHolding -= Time.deltaTime;
            }
            else
            {
                holdingAnimator.SetBool("Holding", false);
                holdedObject.transform.parent = null;
                holdedObject = null;
            }*/
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dragonLayer == (dragonLayer.value | 1 << other.gameObject.layer))
        {
            GameItemBehaviour dragon =  other.GetComponent<GameItemBehaviour>();
            dragon.BecomeBall();
            dragon.SetNextDragonTime(livingTime + 1);

            SetHoldedObject(other.gameObject);
        }
    }

    private void SetHoldedObject(GameObject other)
    {
        holdedObject = other;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if(rb != null)
        {
            Debug.Log("No es null");
            rb.useGravity = false;
            holding = true;
        }
        

        //other.transform.parent = holdingPosition;

        //other.transform.position = Vector3.zero;

        //timeHol

        //holding = true;
        //holdingAnimator.SetBool("Holding", true);

        //holdedObject = other;*/
    }
}
