using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timeToDestroy = 2.5f;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(this, timeToDestroy);
    }
}
