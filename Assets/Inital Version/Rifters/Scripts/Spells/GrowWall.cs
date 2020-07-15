using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowWall : MonoBehaviour
{
    public List<GameObject> inRangeWalls;

    public float limitAngle;
    public float cooldownTime;

    [SerializeField] private LayerMask WallLayer;

    private GameObject targetWall;

    private float nextSpellAllowed;

    // Start is called before the first frame update
    void Awake()
    {
        nextSpellAllowed = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //If the spell is in cooldown you can't activate it
        if (Time.time < nextSpellAllowed)
            return;

        FindClosestWall();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(targetWall != null)
            {
                Debug.Log("Intentando escalar " + targetWall.name);

                targetWall.GetComponent<Wall_Item>().ActivateWall();
            }
            else
            {
                Debug.LogError("Target es nulo");
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (WallLayer == (WallLayer.value | 1 << other.gameObject.layer))
        {
            inRangeWalls.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (WallLayer == (WallLayer.value | 1 << other.gameObject.layer))
        {
            inRangeWalls.Remove(other.gameObject);
        }
    }

    private void FindClosestWall()
    {
        int closestWall = -1;
        float minAngle = Mathf.Infinity;

        //Comprobar las piedras con angulo menor a x
        for (int i = 0; i < inRangeWalls.Count; i++)
        {
            //Conseguir vector dirección hacia la roca
            Vector3 rockPos = inRangeWalls[i].transform.position;
            rockPos.y = transform.position.y;
            Vector3 direction = rockPos - transform.position;

            //Comprobar el angulo entre la roca y el jugador
            float angle = Vector3.Angle(transform.forward, direction);

            //Comprobar si el angulo es menor que el más pequeño y que el angulo limite
            if (angle < minAngle && angle < limitAngle)
            {
                minAngle = angle;
                closestWall = i;
            }
        }

        if (closestWall != -1)
        {
            targetWall = inRangeWalls[closestWall];
        }
    }
}
