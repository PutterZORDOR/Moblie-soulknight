using System;
using UnityEngine;

public class RushEnemy : EnemyBase
{
    public float dashRange = 5f; // Range within which the enemy will start dashing
    public float dashSpeed = 15f; // Speed of the dash
    public int dashDamage = 1; // Damage dealt by the dash
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
        anim.Play("RushDie");
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        if (isDashing)
        {
            Dash();
        }
        else
        {
            MoveTowardsPlayer(); // เดินตามปกติเมื่อไม่ได้พุ่ง
        }

        if (Vector2.Distance(transform.position, player.position) <= dashRange && !isDashing)
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
        // Normal movement towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void StartDash()
    {
        Physics2D.IgnoreCollision(col_Player, col_Enemy, false);
        isDashing = true;
        anim.SetBool("isRunning", true); // เปลี่ยนแอนิเมชัน
        dashDirection = (player.position - transform.position).normalized; // กำหนดทิศทางพุ่ง

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
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            Debug.Log("T");
            PlayerManager.instance.TakeDamgeAll(dashDamage);
            Physics2D.IgnoreCollision(col_Player, col_Enemy, true);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("wall map") && isDashing)
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
