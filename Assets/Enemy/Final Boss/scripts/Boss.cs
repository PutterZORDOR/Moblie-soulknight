using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject bulletPrefab; // The bullet prefab to be instantiated
    public Transform shootPoint;    // The point from where bullets will be shot
    public Transform player;        // Reference to the player
    public float shootingInterval = 2f; // Time interval between shots
    public float bulletSpeed = 10f; // Speed of the bullet
    public float health = 100f; // Boss health

    private float nextShootTime;
    private int attackPatternIndex = 0; // To cycle through attack patterns

    void Start()
    {
        nextShootTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= nextShootTime)
        {
            ExecuteAttackPattern();
            nextShootTime = Time.time + shootingInterval;
            // Optionally, cycle through attack patterns
            attackPatternIndex = (attackPatternIndex + 1) % 5;
        }
    }

    void ExecuteAttackPattern()
    {
        switch (attackPatternIndex)
        {
            case 0:
                ShootAtPlayer(); // Basic single shot
                break;
            case 1:
                SpreadShot(); // Spread shot pattern
                break;
            case 2:
                RapidFire(); // Rapid firing pattern
                break;
            case 3:
                BurstShot(); // Burst shot pattern
                break;
        }
    }

    void ShootAtPlayer()
    {
        if (player == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            Vector3 direction = (player.position - shootPoint.position).normalized;
            bulletRb.linearVelocity = direction * bulletSpeed;
        }
    }

    void SpreadShot()
    {
        if (player == null)
            return;

        int numShots = 5;
        float spreadAngle = 15f;

        for (int i = -numShots / 2; i <= numShots / 2; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                Vector3 direction = Quaternion.Euler(0, i * spreadAngle, 0) * (player.position - shootPoint.position).normalized;
                bulletRb.linearVelocity = direction * bulletSpeed;
            }
        }
    }

    void RapidFire()
    {
        if (player == null)
            return;

        int numShots = 3;
        float delayBetweenShots = 0.1f;

        for (int i = 0; i < numShots; i++)
        {
            Invoke(nameof(ShootAtPlayer), i * delayBetweenShots);
        }
    }

    

    void BurstShot()
    {
        if (player == null)
            return;

        int numShots = 3;
        float angleBetweenShots = 10f;

        for (int i = 0; i < numShots; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                Vector3 direction = Quaternion.Euler(0, i * angleBetweenShots - (numShots - 1) * angleBetweenShots / 2, 0) * (player.position - shootPoint.position).normalized;
                bulletRb.linearVelocity = direction * bulletSpeed;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }
}