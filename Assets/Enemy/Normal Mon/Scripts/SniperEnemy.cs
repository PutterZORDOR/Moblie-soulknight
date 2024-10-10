using System.Collections;
using UnityEngine;

public class SniperEnemy : EnemyBase
{
    public GameObject bulletPrefab; // Assign the bullet prefab in the inspector
    public Transform firePoint; // Assign the fire point transform in the inspector
    public float bulletSpeed = 20f;
    public Animator anim;
    public float fireCooldown = 7f; // Time between shots
    public int bulletDamage = 25;
    public float bulletLifetime = 2f;
    public float shootingRange = 10f; // Distance at which the sniper will stop moving towards the player
    public float randomMoveDistance = 5f; // Maximum distance for random movement

    private float lastFireTime;
    private bool isMovingRandomly = false;
    private float originSpeed;
    private bool isAttack;
    private Vector2 currentDirection; // เพิ่มตัวแปรเพื่อเก็บทิศทางปัจจุบัน

    protected override void Start()
    {

    }
    public override void ResetStat()
    {
        base.Start();
        fireCooldown = Random.Range(6.5f, 9.5f);
        anim = GetComponent<Animator>();
        originSpeed = moveSpeed;
        currentDirection = Random.insideUnitCircle.normalized;

        isAttack = false;
        isMovingRandomly = false;
        isDie = false;
        lastFireTime = 0f;

        anim.SetBool("IsMoving", false);
        anim.ResetTrigger("Attack");
        anim.Play("Sniper_Idle");

        icon.SetActive(false);
        gameObject.tag = "Enemy";
        currentHealth = maxHealth;
    }
    protected override void Update()
    {
        base.Update();
        if (Time.time >= lastFireTime + fireCooldown && !isMovingRandomly)
        {
            icon.SetActive(true);
            StartCoroutine(DelayBeforeAttack());
            lastFireTime = Time.time;
        }

        if (!isMovingRandomly && !isAttack)
        {
            moveSpeed = 1f;
            MaintainDistanceFromPlayer();
        }
    }

    private IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttack = true;
        icon.SetActive(false);
        anim.SetTrigger("Attack");
    }

    // Called by the animation event when the sniper actually fires
    public void ShootPlayer()
    {
        foreach (GameObject bullet in Bullet_Manager_Pool.instance.enemy_Sniper)
        {
            if (!bullet.activeSelf)
            {
                Vector3 direction = (player.position - firePoint.position).normalized;
                bullet.transform.position = firePoint.transform.position;
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.Initialize(direction, bulletDamage, bulletSpeed, bulletLifetime);
                bullet.SetActive(true);
                break;
            }
        }
    }

    public void waitForMove()
    {
        moveSpeed = originSpeed;
        isAttack = false;
        StartCoroutine(MoveRandomlyAfterShooting());
    }

    private IEnumerator MoveRandomlyAfterShooting()
    {
        isMovingRandomly = true;
        anim.SetBool("IsMoving", true);

        // Pick a random direction
        currentDirection = Random.insideUnitCircle.normalized * randomMoveDistance;
        Vector2 targetPosition = (Vector2)transform.position + currentDirection;

        float moveDuration = 2f; // Time spent moving randomly
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMovingRandomly = false; // Finished random movement
        anim.SetBool("IsMoving", false);
    }

    private void MaintainDistanceFromPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > shootingRange)
        {
            MoveTowardsPlayer(); // Move closer to player if too far
        }
        else
        {
            // Stop moving if within shooting range
            anim.SetBool("IsMoving", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        anim.SetBool("IsMoving", true);
        Vector2 direction = (player.position - transform.position).normalized;
        currentDirection = direction;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    protected override void OnPlayerDetected()
    {
        // This could trigger additional behaviors or detection-based actions
    }

    protected override void OnDefeated()
    {
        gameObject.tag = "Untagged";
        anim.Play("Sniper_Die");
    }

    public void DestroySelf()
    {
        isFirstActivation = true;
        gameObject.SetActive(false);
    }

    // ตรวจจับการชนกับกำแพง
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // เปลี่ยนทิศทางการเดินเมื่อชนกำแพง
            currentDirection = -currentDirection;

            // ตรวจสอบว่าติดกำแพงอีกครั้งไหม ถ้าติดให้สุ่มทิศทางใหม่
            if (Physics2D.Raycast(transform.position, currentDirection, 1f, LayerMask.GetMask("Wall")))
            {
                currentDirection = Random.insideUnitCircle.normalized;
            }

            // อัปเดตการเคลื่อนที่
            StartCoroutine(MoveInNewDirection());
        }
    }

    // เคลื่อนที่ไปในทิศทางใหม่
    private IEnumerator MoveInNewDirection()
    {
        float moveDuration = 2f; // กำหนดเวลาเคลื่อนที่
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + currentDirection, moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
