using UnityEngine;
using System.Collections;

public class MaguGenki : MiniBoss
{
    public GameObject swordSlashPrefab; // Prefab for the sword slash wave
    public float swordSlashSpeed = 20f;
    public float swordSlashLifetime = 5f;

    public float dashSpeed = 15f;
    public float dashChargeTime = 3f;

    public float meleeCooldown = 2f;
    public float rangedCooldown = 5f;
    public float dashCooldown = 7f;

    public float meleeDamage = 25f;
    public float rangedDamage = 20f;
    public float dashDamage = 30f;

    private int currentAttackPattern = 0;
    private bool isPatternCooldown = false;

    private Animator anim;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        damage = meleeDamage;

        // Existing code...

        // Add this line to get the Animator component
        anim = GetComponent<Animator>();

        // Start the attack pattern loop
        StartCoroutine(SwitchAttackPatterns());
    }


    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Check if not attacking and should walk
            if (!isPatternCooldown)
            {
                anim.SetBool("isWalking", true);
                // Move towards the player
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

                FlipTowardsPlayer(); // Ensure the boss is facing the player
            }
            else
            {
                anim.SetBool("isWalking", false); // Stop walking during attacks
            }
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 scale = transform.localScale;

            // Check if the player is to the right of the boss
            if (transform.position.x < player.position.x && scale.x < 0)
            {
                scale.x = Mathf.Abs(scale.x); // Flip to face right
            }
            // Check if the player is to the left of the boss
            else if (transform.position.x > player.position.x && scale.x > 0)
            {
                scale.x = -Mathf.Abs(scale.x); // Flip to face left
            }

            transform.localScale = scale;
        }
    }


    private void Update()
    {
        // If the boss is in cooldown, it should still be able to move toward the player
        if (!anim.GetBool("isMelee") && !anim.GetBool("isRanged") && !anim.GetBool("isDashing"))
        {
            MoveTowardsPlayer(); // Keep moving unless performing a specific attack
        }

        if (!isPatternCooldown && !anim.GetBool("isWalking") && !anim.GetBool("isMelee") && !anim.GetBool("isRanged") && !anim.GetBool("isDashing"))
        {
            anim.SetBool("isIdle", true); // Default to idle if not walking or attacking
        }
        else
        {
            anim.SetBool("isIdle", false); // Exit idle when performing other actions
        }
    }



    protected override void ShootPlayer()
    {
        if (isPatternCooldown) return; // Don't attack during cooldown

        FlipTowardsPlayer(); // Ensure the boss is facing the player before attacking

        switch (currentAttackPattern)
        {
            case 0:
                MeleeAttack();
                StartCoroutine(PatternCooldown(meleeCooldown));
                break;
            case 1:
                StartCoroutine(RangedAttack());
                StartCoroutine(PatternCooldown(rangedCooldown));
                break;
            case 2:
                StartCoroutine(DashAttack());
                StartCoroutine(PatternCooldown(dashCooldown));
                break;
        }
    }

    private void MeleeAttack()
    {
        Debug.Log($"{gameObject.name} is performing a melee attack.");
        anim.SetTrigger("isMelee");  // Trigger melee animation
        anim.SetBool("isWalking", false); // Ensure the boss stops walking
    }


    private IEnumerator RangedAttack()
    {
        Debug.Log($"{gameObject.name} is performing a ranged attack.");
        damage = rangedDamage;

        int slashCount = Random.Range(1, 4); // Random number of slashes
        anim.SetInteger("slashCount", slashCount); // Set the count of slashes for the animation

        anim.SetTrigger("isRanged"); // Trigger ranged animation
        anim.SetBool("isWalking", false); // Ensure the boss stops walking

        for (int i = 0; i < slashCount; i++)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Vector3 spawnPosition = transform.position + (Vector3)direction * 0.5f;
            GameObject swordSlash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));

            Rigidbody2D rb = swordSlash.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * swordSlashSpeed;
            }

            Destroy(swordSlash, swordSlashLifetime);

            // Trigger animation for each slash
            anim.SetTrigger("isRanged");

            yield return new WaitForSeconds(0.5f); // Wait before the next slash
        }
    }




    private IEnumerator DashAttack()
    {
        Debug.Log($"{gameObject.name} is charging for a dash attack.");
        damage = dashDamage;

        anim.SetTrigger("isDashing"); // Trigger dash animation
        anim.SetBool("isWalking", false); // Ensure the boss stops walking

        yield return new WaitForSeconds(dashChargeTime);

        int dashCount = Random.Range(1, 4);
        for (int i = 0; i < dashCount; i++)
        {
            FlipTowardsPlayer();
            Vector2 dashDirection = (player.position - transform.position).normalized;
            float dashDuration = 0.5f;
            float dashEndTime = Time.time + dashDuration;

            while (Time.time < dashEndTime)
            {
                transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }


    private IEnumerator SwitchAttackPatterns()
    {
        while (true)
        {
            if (!isPatternCooldown)
            {
                ShootPlayer();
            }

            // Wait for a certain duration before switching patterns
            yield return new WaitForSeconds(Random.Range(5f, 10f)); // Randomly switch between 5 to 10 seconds

            // Switch to the next attack pattern
            currentAttackPattern = (currentAttackPattern + 1) % 3; // Cycle through 0, 1, 2
            Debug.Log($"SamuraiBoss switched to attack pattern {currentAttackPattern}");
        }
    }

    private IEnumerator PatternCooldown(float cooldownDuration)
    {
        Debug.Log($"Cooldown for {cooldownDuration} seconds");
        isPatternCooldown = true;

        // Allow the boss to move even during cooldown
        anim.SetBool("isWalking", true); // Start walking during cooldown if not attacking

        yield return new WaitForSeconds(cooldownDuration);
        isPatternCooldown = false;

        // Make sure the boss keeps moving after the cooldown ends
        anim.SetBool("isWalking", true);
    }

}
