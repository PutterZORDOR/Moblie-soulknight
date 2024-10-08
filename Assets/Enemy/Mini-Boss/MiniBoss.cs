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
    public float maxHealth = 200f; // Added max health
    public float damage;

    public Slider healthSlider; // Reference the UI slider for health

    protected float lastAttackTime;  // Changed to protected
    protected float lastFireTime;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set the slider to the max health at the start
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
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

        // Update health bar value based on current health
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
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
