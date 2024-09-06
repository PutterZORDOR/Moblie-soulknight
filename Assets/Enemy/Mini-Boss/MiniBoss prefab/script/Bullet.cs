using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction; // Direction of the bullet
    public int damage; // Damage value for this bullet
    public float speed; // Speed of the bullet
    public float lifetime; // Lifetime of the bullet

    public void Initialize(Vector3 dir, int dmg, float spd, float life)
    {
        direction = dir;
        damage = dmg;
        speed = spd;
        lifetime = life;
        Destroy(gameObject, lifetime); // Destroy the bullet after its lifetime expires
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
