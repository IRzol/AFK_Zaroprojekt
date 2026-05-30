using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public bool hasGun = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hasGun)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = firePoint.position.z;
        Vector2 direction = ((Vector2)mousePos - (Vector2)firePoint.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;

        Destroy(bullet, 3f);
        hasGun = false;
    }
}