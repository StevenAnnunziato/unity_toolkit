using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{

    [Header("Object References")]
    public Transform baseBulletSpawn;
    public GameObject bulletPrefab;
    public Transform cam;

    [Header("Shooting Settings")]
    public float damage = 34;
    public float maxCooldown = 0.5f;
    public LayerMask bulletMask;

    // private member variables
    private float currentCooldown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // tick cooldown
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;

        // fire
        if (Input.GetButton("Fire1"))
        {
            if (currentCooldown <= 0f)
            {
                Shoot();
            }
        }

    }

    private void Shoot()
    {
        // reset cooldown
        currentCooldown = maxCooldown;

        // instantiate a bullet
        GameObject bullet = Instantiate(bulletPrefab, baseBulletSpawn.position, Quaternion.identity);

        // set damage for this bullet
        bullet.GetComponent<Bullet>().SetDamage(damage);

        // shoot a ray from the camera to hit the exact crosshair,
        // then fire the bullet from the gun towards the ray hit point
        // NOTE: This works better than most AAA third-person shooting systems :)
        RaycastHit hit; Vector3 point;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 999f, bulletMask))
        {
            point = hit.point;
        }
        else
        {
            point = cam.position + cam.forward * 100f;
        }
        bullet.transform.forward = point - baseBulletSpawn.position;
    }
}
