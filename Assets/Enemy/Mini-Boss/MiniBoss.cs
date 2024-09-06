using UnityEngine;
using UnityEngine.UI;

public abstract class MiniBoss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1f;
    public float Range = 1f;
    public float attackCooldown = 3f;
    public float shootingRange = 50f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 5f;
    public float fireRate = 3f;
    public float health = 200f;
    public float damage;
    public GameObject healthBarPrefab; // Assign this in the Inspector
    private GameObject healthBarInstance;
    private Slider healthSlider;

    protected float lastAttackTime;  // Changed to protected
    protected float lastFireTime;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set up health bar, etc.
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > Range)
        {
            MoveTowardsPlayer();
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }

        if (distanceToPlayer <= shootingRange && Time.time >= lastFireTime + fireRate)
        {
            ShootPlayer();
            lastFireTime = Time.time;
        }

        // Update health bar, etc.
    }

    protected void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    protected abstract void ShootPlayer();

    protected virtual void AttackPlayer()
    {
        Debug.Log($"{gameObject.name} is attacking the player.");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    protected void FlipTowardsPlayer()
    {
        Vector3 scale = transform.localScale;
        scale.x = (player.position.x < transform.position.x) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("MiniBoss defeated!");
    }
}
