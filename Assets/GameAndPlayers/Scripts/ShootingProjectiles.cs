using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingProjectiles : MonoBehaviour
{
    public float shootingForce = 30f;
    public float timeBtwProjectiles = .1f;

    public int projectileCount = 6;

    public bool shooting;

    public GameObject projectilePrefab;

    public Vector3 ShootingDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + ShootingDirection, Color.red);
        Debug.DrawLine(transform.position, transform.position + new Vector3(-ShootingDirection.x, ShootingDirection.y, ShootingDirection.z), Color.green);

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
        int side = 1;

        Vector3 direction = new Vector3(ShootingDirection.x * side, ShootingDirection.y, ShootingDirection.z).normalized;
        float rotationdegrees = 90 / (projectileCount - 1);

        for (int i = 0; i < projectileCount; i++)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, rotation);
            projectile.GetComponent<Rigidbody>().AddForce(direction * shootingForce);

            side *= -1;

            direction = Quaternion.AngleAxis(rotationdegrees, transform.forward) * direction;

            yield return new WaitForSeconds(timeBtwProjectiles);
        }

        shooting = false;
        yield return null;
    }
}
