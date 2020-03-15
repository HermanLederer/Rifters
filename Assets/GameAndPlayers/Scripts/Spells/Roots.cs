using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour
{
    public float radius;

    public Transform holdingPosition;

    public float HoldingTime = 4f;
    public float livingTime = 8f;

    private float timeHolding;
    private bool holding;
    private Animator holdingAnimator;

    private GameObject holdedObject;

    // Start is called before the first frame update
    void Start()
    {
        holdingAnimator = holdingPosition.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            if(timeHolding > 0)
            {
                timeHolding -= Time.deltaTime;
            }
            else
            {
                holdingAnimator.SetBool("Holding", false);
                holdedObject.transform.parent = null;
                holdedObject = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            SetHoldedObject(other.gameObject);
        }
    }

    private void SetHoldedObject(GameObject other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        other.transform.parent = holdingPosition;

        other.transform.position = Vector3.zero;

        holding = true;
        holdingAnimator.SetBool("Holding", true);

        timeHolding = HoldingTime;

        holdedObject = other;
    }
}
