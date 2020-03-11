using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dash : MonoBehaviour
{
    private Rigidbody rb;

    public float dashSpeed;
    public float startDashTime;

    private float dashTime;
    private bool dashing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dashing)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                dashing = true;
            }
        }
        else
        {
            if(dashTime <= 0)
            {
                dashing = false;
                dashTime = startDashTime;
                rb.velocity = Vector3.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                rb.velocity = transform.forward * dashSpeed;
            }
        }
    }
}
