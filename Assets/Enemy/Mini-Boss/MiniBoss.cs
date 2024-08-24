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

        // Find the Canvas in the scene
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        // Instantiate the health bar prefab in the canvas
        healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);
        if (healthBarInstance == null)
        {
            Debug.LogError("Health bar prefab could not be instantiated!");
            return;
        }

        healthSlider = healthBarInstance.GetComponentInChildren<Slider>();
        if (healthSlider == null)
        {
            Debug.LogError("Slider component not found in health bar prefab!");
            return;
        }

        // Set the max value of the health slider
        healthSlider.maxValue = health;
        healthSlider.value = health;

        // Position the health bar at the top center of the screen
        RectTransform healthBarRect = healthBarInstance.GetComponent<RectTransform>();
        healthBarRect.anchorMin = new Vector2(0.5f, 1f); // Top center anchor
        healthBarRect.anchorMax = new Vector2(0.5f, 1f); // Top center anchor
        healthBarRect.pivot = new Vector2(0.5f, 1f); // Pivot at the top center
        healthBarRect.anchoredPosition = new Vector2(0, -30f); // Adjust Y position to be slightly below the top

        Debug.Log("Health bar instantiated and positioned successfully.");
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

        // Update the health bar value based on the current health
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Clamp(health, 0, healthSlider.maxValue);
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
