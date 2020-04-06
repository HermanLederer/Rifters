using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBar : MonoBehaviour
{
    public GameObject goalArea1Pos = null;
    public GameObject goalArea2Pos = null;
    public Transform ballImg = null;
    public GameObject ball = null;

    private float distanceBetween;
    private float distanceBetween1;
    private float distanceBetween2;
    void Start()
    {
        goalArea1Pos = GameObject.Find("GoalArea1");
        goalArea2Pos = GameObject.Find("GoalArea2");

        //distanceBetween = Vector3.Distance(goalArea2Pos.transform.position, goalArea1Pos.transform.position);
    }
    void Update()
    {
        if (ball)
        {
            distanceBetween1 = Vector3.Distance(ball.transform.position, goalArea1Pos.transform.position);

            distanceBetween2 = Vector3.Distance(ball.transform.position, goalArea2Pos.transform.position);

            ballImg.localPosition = new Vector3(45 - ((distanceBetween1 * 90) / (distanceBetween1 + distanceBetween2)), 0, 0);
        }
        else
        {
            ballImg.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(AfterBallInstantiation(0.5f));
            ball = GameObject.FindGameObjectWithTag("Dragon");
        }
    }

    IEnumerator AfterBallInstantiation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
