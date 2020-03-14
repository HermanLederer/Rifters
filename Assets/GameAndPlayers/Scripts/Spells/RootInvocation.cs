using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootInvocation : MonoBehaviour
{
    public GameObject plantMarker;
    public LayerMask levelLayer;

    public bool aiming;

    private GameObject marker;
    // Start is called before the first frame update
    void Start()
    {
        marker = Instantiate(plantMarker) as GameObject;
        marker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            aiming = true;
            marker.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            aiming = false;
            marker.SetActive(false);
        }

        if (aiming)
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, levelLayer))
            {
                marker.SetActive(true);
                marker.transform.position = hit.point;
            }
            else
            {
                marker.SetActive(false);
            }
        }
    }
}
