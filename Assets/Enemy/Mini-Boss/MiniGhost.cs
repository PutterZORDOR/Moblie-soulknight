using UnityEngine;
using System.Collections;

public class MiniGhost : MiniBoss
{
    public float minPatternDuration = 3f; // Minimum duration for a pattern
    public float maxPatternDuration = 8f; // Maximum duration for a pattern
    public float invisibilityDuration = 3f; // Duration for invisibility
    public float scratchRange = 5f; // Scratch attack range

    private bool isAttacking = false;
    private int currentPattern = 0;
    private bool isInvisible = false;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f; // Set a unique move speed for the MiniGhost

        // Start the first pattern
        StartCoroutine(SwitchPattern());
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
                gameObject.GetComponent<SpriteRenderer>().enabled = true; // Make the MiniGhost visible
            }
        }
    }

    private IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // Make the MiniGhost invisible
        yield return new WaitForSeconds(invisibilityDuration);
        gameObject.GetComponent<SpriteRenderer>().enabled = true; // Make the MiniGhost visible again
        isInvisible = false;
    }

    protected override void ShootPlayer()
    {
        // The MiniGhost doesn't shoot, so this method can be left empty or used for another attack type if needed
    }

    protected override void AttackPlayer()
    {
        if (!isInvisible)
        {
            base.AttackPlayer();
            Debug.Log("MiniGhost scratches the player!");
        }
    }
}
