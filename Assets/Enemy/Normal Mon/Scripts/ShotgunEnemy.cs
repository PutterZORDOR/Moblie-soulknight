using UnityEngine;

public class ShotgunMon : EnemyBase
{
    public float attackRange = 1.5f; // Range within which the monster can attack
    public float shootRange = 5f; // Range within which the monster can shoot
    public float attackCooldown = 2f; // Cooldown time between attacks
    public GameObject bulletPrefab; // Bullet prefab
    public Transform shootPoint; // Point from where bullets are shot
    public int numberOfBullets = 5; // Number of bullets per shot
    public float spreadAngle = 15f; // Spread angle for shotgun effect
    public float meleeDamage = 20f; // Damage dealt during a melee attack
    public float bulletSpeed = 10f; // Speed of the bullets
    public float bulletLifetime = 2f; // Lifetime of the bullets
    public int bulletDamage = 10; // Damage dealt by each bullet

    private float lastAttackTime;
    private bool playerInRange = false;
    private bool playerInShootRange = false;

    protected override void Start()
    {
        base.Start();

        // Set up detection range using a circle collider
        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
    }

    protected override void Update()
    {
        base.Update();

        if (playerInRange)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer(); // Perform melee attack
            }
            else if (distanceToPlayer <= shootRange)
            {
                playerInShootRange = true;
                ShootPlayer();
            }
            else
            {
                playerInShootRange = false;
                MoveTowardsPlayer();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void ShootPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Vector3 shootDirection = (player.position - shootPoint.position).normalized;

            for (int i = 0; i < numberOfBullets; i++)
            {
                // Calculate bullet direction with spread angle
                float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
                Vector3 direction = Quaternion.Euler(0, 0, angle) * shootDirection;

                // Instantiate bullet with calculated direction
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();

                if (bulletScript != null)
                {
                    bulletScript.Initialize(direction, bulletDamage, bulletSpeed, bulletLifetime); // Use Initialize method
                }
            }

            lastAttackTime = Time.time;
        }
    }

    protected override void OnPlayerDetected()
    {
        playerInRange = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInShootRange = false;
        }
    }

    // Method to deal melee damage to the player
    private void AttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Get the player's health component
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(meleeDamage);
                Debug.Log("ShotgunMon dealt " + meleeDamage + " damage to the player.");
            }

            lastAttackTime = Time.time;
        }
    }
}
