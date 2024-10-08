using UnityEngine;
using System.Collections;

public class MeteorBoss : MiniBoss
{
    public GameObject meteorPrefab;
    public GameObject magmaFloorPrefab;
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to summon
    public float meteorSpread = 7f; // Distance between the meteors
    public float magmaRadius = 15f; // Radius of the magma floor area
    public float meteorShowerRadius = 8f; // Radius for the MeteorShower AOE
    public float patternSwitchMinDuration = 5f; // Min duration for each pattern
    public float patternSwitchMaxDuration = 10f; // Max duration for each pattern
    public float attackRange = 60f; // Range within which the boss will attack
    public float summonInterval = 15f; // Interval to summon random enemies
    public MeteorPool meteorPool;
    
    

    private int currentPattern = 0;
    private bool isAttacking = false; // Track whether the boss is attacking

    protected override void Start()
    {
        base.Start();
        fireRate = 5f;
        damage = 0;
        bulletLifetime = 10f;
        bulletSpeed = 12f;

        // Start the first attack pattern
        StartCoroutine(SwitchPattern());

        // Start the enemy summoning coroutine
        StartCoroutine(SummonRandomEnemies());

        health = 300f;
        maxHealth = 300f;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    protected override void Update()
    {
        base.Update();

        // Check if the boss should flip based on the player's position
        FlipTowardsPlayer();

        // Check distance to player and start attacking if within range
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPatterns());
                    isAttacking = true;
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 scale = transform.localScale;

            if (transform.position.x < player.position.x && scale.x < 0)
            {
                scale.x = Mathf.Abs(scale.x); // Flip to face right
            }
            else if (transform.position.x > player.position.x && scale.x > 0)
            {
                scale.x = -Mathf.Abs(scale.x); // Flip to face left
            }

            transform.localScale = scale;
        }
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }

    private IEnumerator AttackPatterns()
    {
        while (isAttacking)
        {
            ShootPlayer();
            yield return new WaitForSeconds(fireRate); // Adjust the delay between attacks
        }
    }

    protected override void ShootPlayer()
    {
        switch (currentPattern)
        {
            case 0:
                StartCoroutine(SummonMeteorsAtPlayer());
                break;
            case 1:
                MeteorShower();
                break;
            case 2:
                SummonMagmaFloor();
                break;
        }
    }

    private IEnumerator SummonMeteorsAtPlayer()
    {
        int meteorCount = Random.Range(3, 10); // Random number of meteors
        for (int i = 0; i < meteorCount; i++)
        {
            yield return new WaitForSeconds(1f); // 1-second delay

            GameObject meteor = meteorPool.GetObject();
            if (meteor != null)
            {
                meteor.transform.position = new Vector2(player.position.x, player.position.y + 10f);
                Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.down * bulletSpeed;
                }
            }
        }
    }

    private void MeteorShower()
    {
        int meteorCount = Random.Range(7, 20); // Random number of meteors in the area
        for (int i = 0; i < meteorCount; i++)
        {
            GameObject meteor = meteorPool.GetObject();
            if (meteor != null)
            {
                float angle = Random.Range(0f, 360f);
                float radius = Random.Range(0f, meteorShowerRadius);

                Vector2 offset = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius
                );

                meteor.transform.position = (Vector2)transform.position + offset;
                meteor.transform.position = new Vector2(meteor.transform.position.x, meteor.transform.position.y + 10f);

                Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.down * bulletSpeed;
                }
            }
        }
    }

    private void SummonMagmaFloor()
    {
        if (magmaFloorPrefab != null)
        {
            GameObject magmaFloor = Instantiate(magmaFloorPrefab, transform.position, Quaternion.identity);
            // Set the scale of the magma floor to match the desired radius
            magmaFloor.transform.localScale = new Vector3(magmaRadius, magmaRadius, 9f);

            Destroy(magmaFloor, bulletLifetime); // Magma floor lasts for the duration of bulletLifetime
        }
    }


    private IEnumerator SummonRandomEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(summonInterval); // Wait 15 seconds

            for (int i = 0; i < 3; i++) // Summon 3 random enemies
            {
                if (enemyPrefabs.Length > 0)
                {
                    GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                    Vector2 spawnPosition = new Vector2(
                        transform.position.x + Random.Range(-10f, 10f),
                        transform.position.y + Random.Range(-10f, 10f)
                    );
                    Instantiate(randomEnemy, spawnPosition, Quaternion.identity);
                }
            }
        }
    }



private IEnumerator SwitchPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(patternSwitchMinDuration, patternSwitchMaxDuration));
            currentPattern = (currentPattern + 1) % 3; // Cycle through the three patterns
        }
    }

    
}
