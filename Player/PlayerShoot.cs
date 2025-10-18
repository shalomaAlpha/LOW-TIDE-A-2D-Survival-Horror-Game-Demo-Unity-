using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public Camera mainCamera; 
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }
    void Shoot()
    {
        if (mainCamera == null) return;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        Vector2 shootDirection = mousePosition - firePoint.position;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletMovement bulletScript = bullet.GetComponent<BulletMovement>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
        }
    }
}
