using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBar : MonoBehaviour
{
    public Transform bar;
    public float gameTime = 300;
    private float timeMax;
    void Start()
    {
        timeMax = gameTime;
    }

    void Update()
    {
        if(gameTime >= 0)
        {
            gameTime -= Time.deltaTime;
            bar.localScale = new Vector3(gameTime / timeMax, 1f);
        }
    }
}
