using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserInvocation : MonoBehaviour
{
    [Header("Geyser")]
    public GameObject geyserMarker;
    public GameObject geyser;

    public LayerMask levelLayer;

    public bool aiming;

    private GameObject marker;
    private GameObject spell;

    // Start is called before the first frame update
    void Start()
    {
        marker = Instantiate(geyserMarker) as GameObject;
        marker.SetActive(false);
        spell = geyser;
    }

    // Update is called once per frame
    void Update()
    {
        //--Show the marker for the spell--

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

        //--If you are aiming checks if you are hitting a valid part of the level--

        if (aiming)
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, levelLayer) && Vector3.Angle(hit.normal, Vector3.up) < 10)
            {
                marker.SetActive(true);
                marker.transform.position = hit.point;

                //--If you are allowed to create the spell, creates it--

                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(spell, marker.transform.position, Quaternion.identity);
                }
            }
            else
            {
                marker.SetActive(false);
            }
        }
    }
}
