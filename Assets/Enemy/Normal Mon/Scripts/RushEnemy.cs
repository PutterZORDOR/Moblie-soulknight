using System;
using System.Collections;
using UnityEngine;

public class RushEnemy : EnemyBase
{
    public float dashRange = 5f; // Range within which the enemy will start dashing
    public float dashSpeed = 15f; // Speed of the dash
    public int dashDamage;
    public int bleedDamage; // Damage dealt by the dash
    public int timeBetweenBleed;
    public float dashCooldown = 1.5f; // Cooldown time between dashes

    private float lastDashTime;
    private bool isDashing = false;
    private Vector2 dashDirection;

    // Individual stats for the rush enemy
    public int rushHealth = 75; // Health of the rush enemy
    public float rushSpeed = 10f; // Base speed of the rush enemy

    private Animator anim;

    protected override void Start()
    {
        base.Start();
        moveSpeed = rushSpeed; // Set the move speed to the rush speed stat
        anim = GetComponent<Animator>();
    }
    protected override void OnDefeated()
    {
        Physics2D.IgnoreCollision(col_Player, col_Enemy, false);
        gameObject.tag = "Untagged";
        anim.Play("MonRush_Die");
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        if (isDashing & !isDie)
        {
            Dash();
        }
        else
        {
            MoveTowardsPlayer(); // เดินตามปกติเมื่อไม่ได้พุ่ง
        }

        if (Vector2.Distance(transform.position, player.position) <= dashRange && !isDashing & !isDie)
        {
            if (Time.time >= lastDashTime + dashCooldown)
            {
                StartDash();
                lastDashTime = Time.time;
            }
        }
    }

    protected override void OnPlayerDetected()
    {

    }

    void MoveTowardsPlayer()
    {
        if (!isDie)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void StartDash()
    {
        icon.SetActive(true);
        StartCoroutine(DelayBeforeAttack());
    }
    private IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(1f);
        icon.SetActive(false);
        Physics2D.IgnoreCollision(col_Player, col_Enemy, false);
        isDashing = true;
        anim.SetBool("isRunning", true);
        dashDirection = (player.position - transform.position).normalized;
        isDash = true;
    }
    void Dash()
    {
        transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
    }

    private void StopDash()
    {
        isDashing = false; // เปลี่ยนสถานะการพุ่ง
        anim.SetBool("isRunning", false); // หยุดแอนิเมชันการวิ่ง

        isDash = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDashing & !isDie)
        {
            PlayerManager.instance.TakeDamgeAll(dashDamage);
            PlayerManager.instance.StartBleeding(bleedDamage,timeBetweenBleed);
            PlayerManager.instance.ApplyDebuff("Bleeding",timeBetweenBleed);
            Physics2D.IgnoreCollision(col_Player, col_Enemy, true);
            Dash();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("wall map") && isDashing & !isDie)
        {
            Physics2D.IgnoreCollision(col_Player, col_Enemy, true);
            StopDash();
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
