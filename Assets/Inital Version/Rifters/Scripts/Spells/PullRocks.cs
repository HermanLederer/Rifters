using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PullRocks : MonoBehaviour
{
    public List<GameObject> inRangeRocks;
    public Transform holdingTransform;

    public float limitAngle;
    public float shootingForce;
    public float upRotationCameraForward;

    [SerializeField] private LayerMask levelLayer;
    [SerializeField] private string rockTag;

    private GameObject targetRock;

    private bool isHolding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ThrowRock();
            }
        }
        else
        {
            //Comprobar si hay alguna roca que se pueda coger
            FindVisibleRocks();

            if (targetRock != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpRock();
                }
            }
            //Comprobar si el jugador pulsa para cogerla
        }
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

    private void FindVisibleRocks()
    {
        int closestRock = -1;
        float minAngle = Mathf.Infinity;

        //Comprobar las piedras con angulo menor a x
        for (int i = 0; i < inRangeRocks.Count; i++)
        {
            //Conseguir vector dirección hacia la roca
            Vector3 rockPos = inRangeRocks[i].transform.position;
            rockPos.y = transform.position.y;
            Vector3 direction = rockPos - transform.position;

            //Comprobar el angulo entre la roca y el jugador
            float angle = Vector3.Angle(transform.forward, direction);

            //Comprobar si el angulo es menor que el más pequeño y que el angulo limite
            if ( angle < minAngle && angle < limitAngle)
            {
                minAngle = angle;
                closestRock = i;
            }
        }

        if(closestRock != -1)
        {
            targetRock = inRangeRocks[closestRock];
        }
    }

    private void PickUpRock()
    {
        targetRock.transform.SetParent(holdingTransform);

        targetRock.GetComponent<Rigidbody>().isKinematic = true;

        targetRock.transform.DOLocalMove(Vector3.zero, 0.2f);

        isHolding = true;
    }

    private void ThrowRock()
    {
        targetRock.transform.SetParent(null);

        targetRock.GetComponent<Rigidbody>().isKinematic = false;

        Vector3 shootingDirection = Camera.main.transform.forward;
        shootingDirection = Quaternion.AngleAxis(-upRotationCameraForward, Camera.main.transform.right) * shootingDirection;
        shootingDirection.Normalize();
        targetRock.GetComponent<Rigidbody>().AddForce(shootingDirection * shootingForce, ForceMode.Impulse);

        isHolding = false;
    }
}
