using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    public float lifetime = 2f; // Lifetime of the bullet
    private Vector3 direction; // Direction of the bullet
    private int damage; // Damage value for this bullet

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the bullet after its lifetime expires
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Update()
    {
        // Move the bullet in the set direction
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the PlayerHealth component and apply damage
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Use the damage value from the bullet
            }

            Debug.Log("Bullet hit the player!");
            Destroy(gameObject); // Destroy the bullet upon hitting the player
        }
    }
}
