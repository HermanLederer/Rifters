using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    public Transform pushStart;
    public Transform target;

    public float vectorSize = 3f;
    public int angle = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 up = Quaternion.AngleAxis(angle, pushStart.right) * pushStart.forward;
        Vector3 down = Quaternion.AngleAxis(-angle, pushStart.right) * pushStart.forward;
        Vector3 left = Quaternion.AngleAxis(angle, pushStart.up) * pushStart.forward;
        Vector3 right = Quaternion.AngleAxis(-angle, pushStart.up) * pushStart.forward;

        Debug.DrawLine(pushStart.position, pushStart.position + up * vectorSize, Color.green);
        Debug.DrawLine(pushStart.position, pushStart.position + down * vectorSize, Color.green);
        Debug.DrawLine(pushStart.position, pushStart.position + left * vectorSize, Color.green);
        Debug.DrawLine(pushStart.position, pushStart.position + right * vectorSize, Color.green);

        Debug.DrawLine(pushStart.position, pushStart.position + pushStart.forward * vectorSize, Color.blue);
        Debug.DrawLine(pushStart.position, target.position, Color.red);

        Debug.Log("Angle between forward and target: " + Vector3.Angle(pushStart.forward, target.position.normalized));
    }
}
