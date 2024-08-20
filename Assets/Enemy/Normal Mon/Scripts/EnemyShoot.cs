using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform shootPoint; // The point from which the enemy shoots
    public float shootInterval = 2f; // Time between shots

    private float shootTimer;

    void Update()
    {
        // Count down the shoot timer
        shootTimer -= Time.deltaTime;

        // If the timer reaches zero, shoot
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval; // Reset the shoot timer
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the shoot point
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
    }
}
