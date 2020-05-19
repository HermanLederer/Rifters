using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlinkController1 : MonoBehaviour
{
    public float blinkDuration;
    public float blinkDistance;
    public float offsetHeightPlayer;
    public float offsetWidthPlayer;
    public Ease easeType;
    public LayerMask layermask;
    public Slider slider;
    public float maxEnergy;
    public Transform playerOrigin;
    public Animator anim;
    public string blinkTrigger;

    private float currentEnergy;


    // Start is called before the first frame update
    void Awake()
    {
        currentEnergy = maxEnergy / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnergy < maxEnergy)
        {
            currentEnergy += Time.deltaTime;
            if(currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }

        slider.value = currentEnergy / maxEnergy;

        /*if (Input.GetKeyDown(KeyCode.LeftShift) && currentEnergy >= maxEnergy/2)
        {
            anim.SetTrigger(blinkTrigger);
            Blink(blinkDistance);
        }*/
    }

    public bool CheckEnergy()
    {
        return currentEnergy >= maxEnergy / 2;
    }

    public void Blink()
    {
        RaycastHit hit;
        float tpDistance = blinkDistance;

        for (int i = -1; i < 2; i++)
        {
            Vector3 raycastOrigin = transform.position + (offsetHeightPlayer * transform.up * i);

            if(Physics.Raycast(raycastOrigin, transform.forward, out hit, blinkDistance + offsetWidthPlayer, layermask))
            {
                float dist = Mathf.Infinity;

                if(i == 0)
                {
                    dist = Vector3.Distance(transform.position, hit.point) - offsetWidthPlayer;
                }
                else
                {
                    float hipotenusa = Vector3.Distance(transform.position, (hit.point - (offsetWidthPlayer * transform.forward)));
                    float aux = Mathf.Pow(hipotenusa, 2) - Mathf.Pow(offsetHeightPlayer, 2);
                    dist = Mathf.Sqrt(aux);
                }

                if (dist < tpDistance)
                {
                    tpDistance = dist;
                }
            }
        }
        Vector3 destination = transform.position + transform.forward * tpDistance;

        playerOrigin.DOMove(destination, blinkDuration).SetEase(easeType);

        currentEnergy -= maxEnergy / 2;
    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * (blinkDistance + offsetWidthPlayer));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + offsetHeightPlayer * transform.up, transform.position + offsetHeightPlayer * transform.up + transform.forward * (blinkDistance + offsetWidthPlayer));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - offsetHeightPlayer * transform.up, transform.position - offsetHeightPlayer * transform.up + transform.forward * (blinkDistance + offsetWidthPlayer));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * blinkDistance);
    }
}
