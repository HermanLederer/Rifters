using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootInvocation : MonoBehaviour
{
    [Header("Plants")]
    public GameObject plantMarker;
    public GameObject roots;

    [Header("Geyser")]
    public GameObject geyserMarker;
    public GameObject geyser;

    public LayerMask levelLayer;

    public bool aiming;

    private GameObject marker;
    private GameObject markerPlant;
    private GameObject markerGeyser;

    private GameObject spell;
    // Start is called before the first frame update
    void Start()
    {
        markerPlant = Instantiate(plantMarker) as GameObject;
        markerPlant.SetActive(false);
        markerGeyser = Instantiate(geyserMarker) as GameObject;
        markerGeyser.SetActive(false);

        marker = markerPlant;
        marker.SetActive(false);
        spell = roots;
    }

    // Update is called once per frame
    void Update()
    {
        //--Choose Which spell you want to use--

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            marker = markerPlant;
            markerGeyser.SetActive(false);
            spell = roots;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            marker = markerGeyser;
            markerPlant.SetActive(false);
            spell = geyser;
        }

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

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, levelLayer))
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
