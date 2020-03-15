using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public Transform target;

    public float projectileSpeed;
    public float turn;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Dragon").transform; //The ball must have a tag
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.forward * projectileSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Dragon")
        {
            //Instantiate Particle System

            Destroy(gameObject);
        }
    }
}
