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

    private float lastAttackTime;
    private float lastFireTime;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Instantiate the health bar prefab and set it as a child of the miniboss
        healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBarInstance.transform.SetParent(transform, false);
        healthSlider = healthBarInstance.GetComponentInChildren<Slider>();

        // Set the max value of the health slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }

        // Adjust the position of the health bar relative to the miniboss
        RectTransform healthBarRect = healthBarInstance.GetComponent<RectTransform>();
        healthBarRect.anchoredPosition = new Vector2(0, 1.5f); // Adjust this value to place it above the miniboss
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
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

        // Update the health bar position above the miniboss
        if (healthBarInstance != null)
        {
            Vector3 healthBarPosition = transform.position + new Vector3(0, 2f, 0); // Adjust Y offset if needed
            healthBarInstance.transform.position = healthBarPosition;

            // Reset the rotation to avoid it following the miniboss's rotation
            healthBarInstance.transform.rotation = Quaternion.identity;
        }
    }

    protected void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    protected virtual void AttackPlayer()
    {
        // Implement attack logic here if needed
    }

    protected abstract void ShootPlayer();

    public void TakeDamage(float damage)
    {
        Debug.Log("MiniBoss took damage: " + damage);
        health -= damage;
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Clamp(health, 0, healthSlider.maxValue);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Debug.Log("MiniBoss has been defeated!");
        // You can add any additional logic here for when the MiniBoss is defeated
    }
}
