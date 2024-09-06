using UnityEngine;

public class SniperEnemy : EnemyBase
{
    public GameObject bulletPrefab; // Assign the bullet prefab in the inspector
    public Transform firePoint; // Assign the fire point transform in the inspector
    public float bulletSpeed = 20f;
    public float fireCooldown = 7f; // Time between shots
    public int bulletDamage = 25;

    private float lastFireTime;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f; // Sniper moves slower but shoots at range

    }

    protected override void Update()
    {
        base.Update();
        if (Time.time >= lastFireTime + fireCooldown)
        {
            ShootPlayer();
            lastFireTime = Time.time;
        }
    }

    protected override void OnPlayerDetected()
    {
        MoveTowardsPlayer(); // Optionally move towards the player if needed
    }

    private void ShootPlayer()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
        }

        Debug.Log("Sniper fired a shot!");
    }

    private void MoveTowardsPlayer()
    {
        // Sniper can optionally move towards player if needed
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}
