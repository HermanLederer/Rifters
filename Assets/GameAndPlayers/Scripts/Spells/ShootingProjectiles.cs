using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingProjectiles : MonoBehaviour
{
    public float shootingForce = 30f;
    public float timeBtwProjectiles = .1f;

    public Transform characterModel;

    public int projectileCount = 6;

    public bool shooting;

    public GameObject projectilePrefab;

    public float offsetX = 1;
    public float offsetY = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 shootingPos = characterModel.position + (characterModel.right * offsetX + characterModel.up * offsetY);

        Debug.DrawLine(transform.position, shootingPos, Color.red);
        if (!shooting)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                shooting = true;
                StartCoroutine(ShootProjectiles());
            }
        }
    }

    IEnumerator ShootProjectiles()
    {
        Vector3 shootingPos = characterModel.position + (characterModel.right * offsetX + characterModel.up * offsetY);

        Vector3 direction = shootingPos - transform.position;

        float angle = Vector3.Angle(transform.up, direction);

        float rotationdegrees = (angle * 2) / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, rotation);
            projectile.GetComponent<Rigidbody>().AddForce(direction * shootingForce);

            direction = Quaternion.AngleAxis(rotationdegrees, characterModel.forward) * direction;

            yield return new WaitForSeconds(timeBtwProjectiles);
        }

        shooting = false;
        yield return null;
    }
}
