using UnityEngine;
using System.Collections;

public class Ghost : MiniBoss
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to summon
    public float minPatternDuration = 3f; // Minimum duration for a pattern
    public float maxPatternDuration = 8f; // Maximum duration for a pattern
    public float invisibilityDuration = 3f; // Duration for invisibility
    public float summonInterval = 10f; // Interval to summon random enemies
    public float minSummonCount = 4; // Minimum number of enemies to summon
    public float maxSummonCount = 7; // Maximum number of enemies to summon
    public float scratchRange = 5f; // Scratch attack range

    private bool isAttacking = false;
    private int currentPattern = 0;
    private bool isInvisible = false;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f; // Set a unique move speed for the Ghost

        // Start the first pattern
        StartCoroutine(SwitchPattern());

        // Start summoning enemies
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

        if (!isInvisible)
        {
            FlipTowardsPlayer();
        }

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= scratchRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    private IEnumerator SwitchPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPatternDuration, maxPatternDuration));

            currentPattern = Random.Range(0, 2); // Randomly choose between invisibility and normal behavior

            if (currentPattern == 0) // Invisibility pattern
            {
                StartCoroutine(BecomeInvisible());
            }
            else
            {
                isInvisible = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true; // Make the Ghost visible
            }
        }
    }

    private IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // Make the Ghost invisible
        yield return new WaitForSeconds(invisibilityDuration);
        gameObject.GetComponent<SpriteRenderer>().enabled = true; // Make the Ghost visible again
        isInvisible = false;
    }

    private IEnumerator SummonRandomEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(summonInterval);

            int summonCount = Random.Range((int)minSummonCount, (int)maxSummonCount + 1);

            for (int i = 0; i < summonCount; i++)
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

    protected override void ShootPlayer()
    {
        // The Ghost doesn't shoot, so this method can be left empty or used for another attack type if needed
    }

    protected override void AttackPlayer()
    {
        if (!isInvisible)
        {
            base.AttackPlayer();
            Debug.Log("Ghost scratches the player!");
        }
    }
}
