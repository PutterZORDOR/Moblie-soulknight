using UnityEngine;

public class MageEnemy : EnemyBase
{
    public GameObject magicOrbPrefab; // Assign the magic orb prefab in the inspector
    public Transform firePoint; // Assign the fire point in the inspector
    public float summonCooldown = 5f; // Time between summoning orbs
    public float orbSpeed = 5f; // Speed of the homing orb
    public float orbLifetime = 10f; // Lifetime of the orb
    public int orbDamage = 30;

    private float lastSummonTime;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 1.5f; // Mage moves slower but has powerful magic attacks
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time >= lastSummonTime + summonCooldown)
        {
            SummonMagicOrb();
            lastSummonTime = Time.time;
        }
    }

    protected override void OnPlayerDetected()
    {
        MoveTowardsPlayer(); // Optionally move towards the player if needed
    }

    private void SummonMagicOrb()
    {
        GameObject orb = Instantiate(magicOrbPrefab, firePoint.position, Quaternion.identity);
        HomingOrb homingOrbScript = orb.GetComponent<HomingOrb>();

        if (homingOrbScript != null)
        {
            homingOrbScript.SetTarget(player);
            homingOrbScript.SetSpeed(orbSpeed);
            homingOrbScript.SetDamage(orbDamage);
            Destroy(orb, orbLifetime); // Destroy the orb after its lifetime ends
        }

        Debug.Log("Mage summoned a homing magic orb!");
    }

    private void MoveTowardsPlayer()
    {
        // Mage can optionally move towards the player if needed
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}
