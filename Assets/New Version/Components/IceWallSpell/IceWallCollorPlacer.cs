using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallCollorPlacer : MonoBehaviour
{
    public LayerMask PlayerAndDragon;

    [Header("Materials")]
    public Material normal;
    public Material colliding;
    public Material wrong;

    private MeshRenderer mesh;
    private int objectsInTrigger;

    
    // Start is called before the first frame update
    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(objectsInTrigger > 0)
        {
            mesh.material = colliding;
        }
        else
        {
            mesh.material = normal;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //--If there is an object in the collider adds it to the counter--
        if (PlayerAndDragon == (PlayerAndDragon.value | 1 << other.gameObject.layer))
        {
            objectsInTrigger++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //--If an object exits the collider removes it from the counter--
        if (PlayerAndDragon == (PlayerAndDragon.value | 1 << other.gameObject.layer))
        {
            objectsInTrigger--;
        }
    }
}
