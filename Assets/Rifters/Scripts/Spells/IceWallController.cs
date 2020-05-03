using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceWallController : MonoBehaviour
{
    public GameObject wall; //Wall model

    public float wallHeight;
    public float scaleTime;
    public float lifeTime; //Time the wall will be placed if does not hit anything
    public float destroyTime; //Lifetime of the wall while cracking

    public LayerMask objectsLayerMask; //Objects the wall can freeze

    //private bool somethingIsTrapped;
    private float _lifeTime; //Remaining lifetime of the wall

    // Start is called before the first frame update
    void Awake()
    {
        //--Moving and scaling the wall through tweens--
        wall.transform.DOScaleY(wallHeight, scaleTime);
        wall.transform.DOMoveY(wallHeight / 2, scaleTime);

        //--Parameters for the box cast--
        Vector3 boxCastPosition = transform.position + Vector3.up * (wallHeight / 2); //Box position
        Quaternion dir = Quaternion.LookRotation(transform.forward); //Box orientation

        //--Box Cast--
        Collider[] colls = Physics.OverlapBox(boxCastPosition, new Vector3(4.5f, 4f, 1f), dir, objectsLayerMask);

        //--If the cast hits something--
        if(colls.Length > 0)
        {
            Debug.Log("Le he dado a alguien");
            //somethingIsTrapped = true;
            _lifeTime = .5f;
        }
        //--If the cast does not hit anything--
        else
        {
            _lifeTime = lifeTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_lifeTime > 0)
        {
            _lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject, destroyTime);
        }
    }

    //--If the wall is hit with a fireball this will be called--
    public void HitWithFireBall() 
    {
        Destroy(gameObject);
    }
}
