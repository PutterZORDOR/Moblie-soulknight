using UnityEngine;

public class MageEnemy : EnemyBase
{
    public GameObject magicOrbPrefab; // Assign the magic orb prefab in the inspector
    public Transform firePoint; // Assign the fire point in the inspector
    public float summonCooldown = 5f; // Time between summoning orbs
    public float orbSpeed = 5f; // Speed of the homing orb
    public float orbLifetime = 10f; // Lifetime of the orb
    public int orbDamage = 30;
    public Animator anim;

    private float lastSummonTime;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
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
        if (playerDetected)
        {
            MoveTowardsPlayer();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void SummonMagicOrb()
    {
        anim.SetTrigger("Attack");
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
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // เช็คว่าตำแหน่งใหม่แตกต่างจากตำแหน่งปัจจุบันหรือไม่
        if (newPosition != (Vector2)transform.position)
        {
            anim.SetBool("isWalking", true); // เล่นแอนิเมชันเดิน
        }
        else
        {
            anim.SetBool("isWalking", false); // หยุดแอนิเมชันเดิน
        }

        transform.position = newPosition;

    }
}
