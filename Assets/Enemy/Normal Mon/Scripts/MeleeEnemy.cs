using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float attackRange = 1.5f; // Range within which the enemy can attack
    public float secondaryDetectionRange = 5f; // Range within which icon will show
    public float attackCooldown = 2f; // Cooldown time between attacks
    public float retreatDuration = 1f; // Time for retreating after a missed attack
    public float retreatDistance = 2f; // Distance to retreat after a missed attack

    private float lastAttackTime;
    public bool isAttack;
    public bool isRetreating = false;
    private bool isAttacking = false; // ตัวแปรสำหรับเช็คการโจมตี
    private float retreatStartTime;
    private Vector2 retreatDirection;
    private bool playerInRange = false;

    [Header("Setting Retreating")]
    public float retreatDurationMin = 0.5f; // Minimum retreat duration
    public float retreatDurationMax = 2f; // Maximum retreat duration
    public float retreatDistanceMin = 1f; // Minimum retreat distance
    public float retreatDistanceMax = 3f; // Maximum retreat distance

    public int meleeDamage; // Damage dealt by the melee enemy
    public float meleeSpeed = 1f; // Speed of the melee enemy

    public Transform attackPoint;
    public Vector2 attackSize = new Vector2(2f, 1f);

    public Animator anim;

    protected override void Start()
    {
        base.Start();
        attackCooldown = Random.Range(3.2f, 4.5f);
        anim = GetComponent<Animator>();
    }
    void Initialize()
    {
        anim.Play("MonMeleeIdel");
        anim.SetBool("isWalking", false);
        anim.ResetTrigger("Attack");

        isDie = false;
        isAttack = false;
        isAttacking = false;
        isRetreating = false;
        playerInRange = false;
        icon.SetActive(false);
        lastAttackTime = Time.time - attackCooldown;

        gameObject.tag = "Enemy";
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        if(isFirstActivation)
        {
            Initialize();
            isFirstActivation = false;
        }
        // ตรวจสอบว่ากำลังถอยหรือไม่
        if (isRetreating)
        {
            Retreat(); // เรียกฟังก์ชัน Retreat ภายใน Update
            return;
        }

        // หยุดการเคลื่อนไหวระหว่างการโจมตี
        if (isAttacking)
        {
            anim.SetBool("isWalking", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ตรวจสอบว่าผู้เล่นอยู่ในระยะวงที่สองหรือไม่
        if (distanceToPlayer <= secondaryDetectionRange && !isDie)
        {
            icon.SetActive(true);
        }
        else
        {
            icon.SetActive(false);
        }

        if (playerInRange)
        {
            // เคลื่อนไปหาผู้เล่นถ้าอยู่นอกระยะโจมตี
            if (distanceToPlayer > attackRange && !isDie)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (Time.time >= lastAttackTime + attackCooldown && !isDie)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time; // อัปเดตเวลาการโจมตีล่าสุด
                }
            }
        }
        else
        {
            anim.SetBool("isWalking", false); // หยุดการเคลื่อนไหวถ้าผู้เล่นไม่อยู่ในระยะ
        }
    }

    protected override void OnPlayerDetected()
    {
        playerInRange = true;
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player
        if (!isAttack && !isDie)
        {
            // Calculate new position
            Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Check if the enemy is moving
            if (newPosition != (Vector2)transform.position)
            {
                anim.SetBool("isWalking", true); // Play walking animation if moving
            }
            else
            {
                anim.SetBool("isWalking", false); // Stop walking animation if not moving
            }

            // Move the enemy in the direction of the player
            transform.position = newPosition;
        }
    }

    void AttackPlayer()
    {
        isAttacking = true;
        icon.SetActive(false);
        anim.SetBool("isWalking", false);
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            anim.SetTrigger("Attack"); // Play attack animation
        }
    }

    public void OnAttackComplete()
    {
        isAttacking = false;
        StartRetreat();
    }

    public void SwingSword()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f);

        foreach (Collider2D hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerManager.instance.TakeDamgeAll(meleeDamage);
            }
        }
    }

    void StartRetreat()
    {
        if (!isDie)
        {
            isRetreating = true;
            retreatStartTime = Time.time;

            // Randomize retreat duration and distance
            float randomRetreatDuration = Random.Range(retreatDurationMin, retreatDurationMax);
            float randomRetreatDistance = Random.Range(retreatDistanceMin, retreatDistanceMax);

            // Calculate retreat direction
            retreatDirection = (transform.position - player.position).normalized;

            // Set retreat distance and duration
            retreatDuration = randomRetreatDuration;
            retreatDistance = randomRetreatDistance;
        }
    }

    void Retreat()
    {
        if (Time.time < retreatStartTime + retreatDuration && !isDie)
        {
            // Move the enemy in the retreat direction
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + retreatDirection * retreatDistance, moveSpeed * Time.deltaTime);
            anim.SetBool("isWalking", true); // Play walking animation while retreating
        }
        else
        {
            // Stop retreating when time is up
            isRetreating = false;

            anim.SetBool("isWalking", false); // Stop walking animation when retreat ends

            // Check if the player is still in detection range
            if (Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                playerInRange = true; // Resume detection and attack if the player is still in range
            }
        }
    }

    protected override void OnDefeated()
    {
        gameObject.tag = "Untagged";
        anim.Play("MonMeleeDie");
    }

    public void DestroySelf()
    {
        isFirstActivation = true;
        gameObject.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, secondaryDetectionRange);
    }

}
