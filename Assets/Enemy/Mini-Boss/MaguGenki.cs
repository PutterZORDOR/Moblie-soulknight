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


    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f; // Adjust this value as needed
        damage = meleeDamage; // Initial damage for melee attack

        // Start the attack pattern loop
        StartCoroutine(SwitchAttackPatterns());
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
        // Implement melee attack logic using the sword collider here
        // Example: Apply melee damage to the player
    }

    private IEnumerator RangedAttack()
    {
        Debug.Log($"{gameObject.name} is performing a ranged attack.");
        damage = rangedDamage;

        int slashCount = Random.Range(1, 4);
        for (int i = 0; i < slashCount; i++)
        {
            // Calculate the direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Calculate the rotation angle to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Instantiate the sword slash projectile slightly in front of the boss
            Vector3 spawnPosition = transform.position + (Vector3)direction * 0.5f;
            GameObject swordSlash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));

            // Get the Rigidbody2D component
            Rigidbody2D rb = swordSlash.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply the direction to the Rigidbody2D velocity
                rb.velocity = direction * swordSlashSpeed;

                // Debug logs to verify direction
                Debug.Log($"Sword slash {i + 1} direction: {direction}");

                // Draw a line in the Scene view for visualization
                Debug.DrawLine(spawnPosition, player.position, Color.red, 2f);
            }
            else
            {
                Debug.LogError("Rigidbody2D component not found on swordSlashPrefab.");
            }

            // Destroy the sword slash after its lifetime
            Destroy(swordSlash, swordSlashLifetime);

            // Wait before firing the next slash
            yield return new WaitForSeconds(0.5f); // Adjust the delay between slashes as needed
        }
    }



    private IEnumerator DashAttack()
    {
        Debug.Log($"{gameObject.name} is charging for a dash attack.");
        damage = dashDamage; // Set damage for dash attack

        yield return new WaitForSeconds(dashChargeTime); // Charge time before the dash

        int dashCount = Random.Range(1, 4); // Randomly choose between 1 to 3 dashes
        for (int i = 0; i < dashCount; i++)
        {
            FlipTowardsPlayer(); // Ensure the boss faces the player before each dash

            Vector2 dashDirection = (player.position - transform.position).normalized;
            float dashDuration = 0.5f; // Adjust the dash duration as needed
            float dashEndTime = Time.time + dashDuration;

            while (Time.time < dashEndTime)
            {
                transform.position += (Vector3)dashDirection * dashSpeed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f); // Pause between each dash
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
        yield return new WaitForSeconds(cooldownDuration);
        isPatternCooldown = false;
    }
}
