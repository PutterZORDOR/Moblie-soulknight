using UnityEngine;

public class RushEnemy : EnemyBase
{
    public float dashRange = 5f; // Range within which the enemy will start dashing
    public float dashSpeed = 15f; // Speed of the dash
    public int dashDamage = 20; // Damage dealt by the dash
    public float dashCooldown = 1.5f; // Cooldown time between dashes

    private float lastDashTime;
    private bool isDashing = false;
    private Vector2 dashDirection;

    // Individual stats for the rush enemy
    public int rushHealth = 75; // Health of the rush enemy
    public float rushSpeed = 10f; // Base speed of the rush enemy

    private Animator animator;
    private bool isFacingRight = true;
    private Collider2D col;

    protected override void Start()
    {
        base.Start();
        moveSpeed = rushSpeed; // Set the move speed to the rush speed stat
        currentHealth = rushHealth; // Set the initial health to the rush health stat

        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;

        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (isDashing)
        {
            Dash();
            return;
        }

        // Check if the player is within the detection range
        if (Vector2.Distance(transform.position, player.position) <= dashRange)
        {
            if (Time.time >= lastDashTime + dashCooldown)
            {
                StartDash();
                lastDashTime = Time.time;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    protected override void OnPlayerDetected()
    {
        // Logic when the player is detected
    }

    void MoveTowardsPlayer()
    {
        // Normal movement towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void StartDash()
    {
        col.isTrigger = true;
        animator.SetBool("isRunning", true);
        isDashing = true;
        dashDirection = (player.position - transform.position).normalized;
    }

    void Dash()
    {
        transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;

        // Check if the enemy has passed the player or needs to stop dashing
        if (Vector2.Distance(transform.position, player.position) > dashRange)
        {
            col.isTrigger = false;
            isDashing = false;
            animator.SetBool("isRunning", false);

            MoveTowardsPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDashing)
        {
            Debug.Log("Rush enemy dashes through the player!");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dashDamage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        // Set the color for the detection range (yellow)
        Gizmos.color = Color.yellow;
        // Draw the detection range as a wire sphere
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Set the color for the dash range (red)
        Gizmos.color = Color.red;
        // Draw the dash range as a wire sphere
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }
}


