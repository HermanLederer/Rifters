using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObject : InteractibleLevel
{
    public float moveSpeed = 5f; //Pull

    [Header("Particle Systems")]
    public GameObject PullPS; //Pull

    private void Awake()
    {
        typeOfObject = TypeOfInteractableObject.PULL;
        rb = GetComponent<Rigidbody>();
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
            transform.position = Vector3.Lerp(transform.position, myPlayer.ObjectHolder.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public override void ActivateObject(PlayerSpells ps)
    {
        if (!activated)
        {
            PullToPlayer(ps);
        }
        else
        {
            ThrowObject(ps);
        }
    }

    private void PullToPlayer(PlayerSpells ps)
    {
        GameObject particles = Instantiate(PullPS, transform.position, Quaternion.identity);
        Destroy(particles, 2f);

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
}
