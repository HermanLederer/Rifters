using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall_Item : MonoBehaviour
{
    public float scaleTime;

    public float scaleFactorX;
    public float scaleFactorY;
    public float scaleFactorZ;

    private bool activated;

    private float nextActivationTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nextActivationTime < Time.time && activated)
            activated = false;
    }

    public void ActivateWall()
    {
        if (!activated)
        {
            /*
            transform.DOScaleX(scaleFactorX, scaleTime).SetLoops(2, LoopType.Yoyo);
            transform.DOScaleY(scaleFactorY, scaleTime).SetLoops(1, LoopType.Yoyo);
            transform.DOScaleZ(scaleFactorZ, scaleTime).SetLoops(1, LoopType.Yoyo);
            */

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(transform.DOScaleY(scaleFactorY, .5f));
            mySequence.Append(transform.DOScaleY(scaleFactorY, scaleTime));
            mySequence.Append(transform.DOScaleY(1, .5f));

            nextActivationTime = Time.time + scaleTime + 1.5f;

            activated = true;
        }
    }
}
