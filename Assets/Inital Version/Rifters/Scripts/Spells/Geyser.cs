using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public float upForce;
    public float livingTime = 2;
    public float ballTime;

    public LayerMask dragonLayer;

    private float lifeTime;

    // Start is called before the first frame update
    void Awake()
    {
        lifeTime = livingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
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
