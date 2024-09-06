using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected Transform player;
    public float detectionRange = 10f;
    public float moveSpeed;

    public int maxHealth = 100; // Maximum health for the enemy
    protected int currentHealth; // Current health of the enemy

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth; // Initialize current health
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectPlayer();
    }

    protected void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            OnPlayerDetected();
        }
    }

    // Method to be overridden in child classes for specific behaviors
    protected abstract void OnPlayerDetected();

    // Method to take damage
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage!");

        if (currentHealth <= 0)
        {
            OnDefeated();
        }
    }

    // Method to handle when the enemy is defeated
    protected virtual void OnDefeated()
    {
        Debug.Log($"{gameObject.name} defeated!");
        Destroy(gameObject); // Destroy the enemy game object
    }
}
