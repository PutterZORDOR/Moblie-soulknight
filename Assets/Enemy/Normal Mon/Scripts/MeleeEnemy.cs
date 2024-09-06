using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float attackRange = 1.5f; // Range within which the enemy can attack
    public float attackCooldown = 2f; // Cooldown time between attacks
    public float retreatDuration = 1f; // Time for retreating after a missed attack
    public float retreatDistance = 2f; // Distance to retreat after a missed attack

    private float lastAttackTime;
    private bool isRetreating = false;
    private float retreatStartTime;
    private Vector2 retreatDirection;
    private bool playerInRange = false;

    // Individual stats for the melee enemy
    public int meleeHealth = 100; // Health of the melee enemy
    public int meleeDamage = 10; // Damage dealt by the melee enemy
    public float meleeSpeed = 5f; // Speed of the melee enemy

    protected override void Start()
    {
        base.Start();
        moveSpeed = meleeSpeed; // Set the move speed to the melee speed stat
        currentHealth = meleeHealth; // Set the initial health to the melee health stat

        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
    }

    protected override void Update()
    {
        base.Update();

        if (isRetreating)
        {
            Retreat();
            return;
        }

        // Check if the player is within the detection range
        if (playerInRange)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Move towards the player if outside attack range
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                // Attack the player if within range and cooldown has elapsed
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time; // Update the last attack time

                    // Start retreating after attack
                    StartRetreat();
                }
            }
        }
    }

    protected override void OnPlayerDetected()
    {
        playerInRange = true;
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the enemy in the direction of the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        // Check if the player is within the attack range
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            // Implement your attack logic here
            Debug.Log("Melee enemy attacks the player!");

            // Assuming the player has a health system
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
            }
        }
    }

    void StartRetreat()
    {
        isRetreating = true;
        retreatStartTime = Time.time;

        // Calculate retreat direction opposite to the player
        retreatDirection = (transform.position - player.position).normalized;
    }

    void Retreat()
    {
        if (Time.time < retreatStartTime + retreatDuration)
        {
            // Move the enemy in the retreat direction
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + retreatDirection * retreatDistance, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Stop retreating after the duration has elapsed
            isRetreating = false;

            // Check if the player is still in detection range
            if (Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                playerInRange = true; // Allow the enemy to attack again if the player is detected
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left detection range!");
            playerInRange = false;
        }
    }
}
