using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    public float attackRange = 1.5f; // Range within which the enemy can attack
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

    // Individual stats for the melee enemy
    public int meleeHealth = 100; // Health of the melee enemy
    public int meleeDamage; // Damage dealt by the melee enemy
    public float meleeSpeed = 1f; // Speed of the melee enemy

    public Transform attackPoint;
    public Vector2 attackSize = new Vector2(2f, 1f);

    public Animator anim;

    protected override void Start()
    {
        base.Start();
        moveSpeed = meleeSpeed; // Set the move speed to the melee speed stat
        currentHealth = meleeHealth; // Set the initial health to the melee health stat
        anim = GetComponent<Animator>();

        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
    }

    protected override void Update()
    {
        base.Update();

        // ตรวจสอบว่ากำลังถอยหรือไม่
        if (isRetreating)
        {
            Retreat(); // เรียกฟังก์ชัน Retreat ภายใน Update
            return;
        }

        // หยุดการเคลื่อนไหวระหว่างการโจมตี
        if (isAttacking)
        {
            anim.SetBool("isWalking", false); // หยุด animation เดิน
            return;
        }

        // ตรวจสอบว่าผู้เล่นอยู่ในระยะการตรวจจับหรือไม่
        if (playerInRange)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // เคลื่อนไปหาผู้เล่นถ้าอยู่นอกระยะโจมตี
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                // โจมตีผู้เล่นถ้าอยู่ในระยะและ cooldown ผ่านแล้ว
                if (Time.time >= lastAttackTime + attackCooldown)
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
        if (!isAttack)
        {
            Vector2 direction = (player.position - transform.position).normalized;

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
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackSize,0f);

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
        isRetreating = true;
        retreatStartTime = Time.time;

        // คำนวณทิศทางถอยหลัง
        retreatDirection = (transform.position - player.position).normalized;
    }

    void Retreat()
    {
        if (Time.time < retreatStartTime + retreatDuration)
        {
            // เคลื่อนศัตรูในทิศทางถอยหลัง
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + retreatDirection * retreatDistance, moveSpeed * Time.deltaTime);
            anim.SetBool("isWalking", true); // เล่น animation เดินขณะถอย
        }
        else
        {
            // หยุดถอยหลังเมื่อเวลาหมด
            isRetreating = false;

            anim.SetBool("isWalking", false); // หยุด animation เดินเมื่อการถอยสิ้นสุด

            // ตรวจสอบว่าผู้เล่นยังอยู่ในระยะตรวจจับหรือไม่
            if (Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                playerInRange = true; // เริ่มตรวจจับและโจมตีใหม่หากผู้เล่นยังอยู่ในระยะ
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
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

}
