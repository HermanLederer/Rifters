using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public float upForce;

    public float ballTime;

    public LayerMask dragonLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(dragonLayer == (dragonLayer.value | 1 << other.gameObject.layer))
        {
            GameItemBehaviour dragon = other.GetComponent<GameItemBehaviour>();

            dragon.BecomeBall();
            dragon.SetNextDragonTime(ballTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (dragonLayer == (dragonLayer.value | 1 << other.gameObject.layer))
        {
            if (other.attachedRigidbody)
            {
                other.attachedRigidbody.AddForce(Vector3.up * upForce);
            }
        }
    }

}
