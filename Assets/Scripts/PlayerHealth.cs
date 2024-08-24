using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Destroy the player object
        Destroy(gameObject);
        // Implement any additional death logic, such as game over screen, respawn, etc.
    }
}
