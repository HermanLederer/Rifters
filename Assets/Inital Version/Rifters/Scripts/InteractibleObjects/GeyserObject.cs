using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserObject : InteractibleLevel
{
    public InteractibleObject geyserSO;

    public float upForce;
    public float livingTime = 2;
    public float ballTime;

    public List<MeshRenderer> meshes;

    public LayerMask dragonLayer;

    public GameObject GeyserPS;

    private float lifeTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        GeyserPS.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            activated = false;
            GeyserPS.SetActive(false);
        }
    }

    public override void ActivateObject(PlayerSpells ps)
    {
        if (!activated)
        {
            activated = true;
            lifeTime = livingTime;
            GeyserPS.SetActive(true);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (dragonLayer == (dragonLayer.value | 1 << other.gameObject.layer))
        {
            GameItemBehaviour dragon = other.GetComponent<GameItemBehaviour>();

            dragon.BecomeBall();
            dragon.SetNextDragonTime(ballTime);
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (dragonLayer == (dragonLayer.value | 1 << other.gameObject.layer))
        {
            GameItemBehaviour dragon = other.GetComponent<GameItemBehaviour>();

            dragon.SetNextDragonTime(ballTime);

            if (other.attachedRigidbody)
            {
                other.attachedRigidbody.AddForce(Vector3.up * upForce);
            }
        }
    }

    public override void SetNormalMaterial()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = geyserSO.normalMat;
        }
        
    }

    public override void SetFresnelMaterial()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = geyserSO.fresnelMat;
        }
    }
}
