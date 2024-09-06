using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public float damage = 20f;       // Damage dealt by the sword slash
    public float speed = 20f;        // Speed of the sword slash
    public float lifetime = 5f;      // Lifetime of the sword slash before it's destroyed

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // Destroy the sword slash after its lifetime expires
    }

    void Update()
    {
        // The slash will move forward automatically; ensure the Rigidbody2D is set to kinematic
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the sword slash hits the player, deal damage
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"SwordSlash dealt {damage} damage to the player.");
            }

            // Destroy the sword slash after it hits the player
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Optionally, destroy the sword slash if it hits an obstacle
            Destroy(gameObject);
        }
    }
}
