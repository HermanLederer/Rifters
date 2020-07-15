using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullRocks : MonoBehaviour
{
    public List<GameObject> inRangeRocks;
    private List<GameObject> visibleRocks;

    [SerializeField] private LayerMask levelLayer;
    [SerializeField] private string rockTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hay " + inRangeRocks.Count + " rocas en la lista");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (levelLayer == (levelLayer.value | 1 << other.gameObject.layer))
        {
            inRangeRocks.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (levelLayer == (levelLayer.value | 1 << other.gameObject.layer))
        {
            inRangeRocks.Remove(other.gameObject);
        }
    }
}
