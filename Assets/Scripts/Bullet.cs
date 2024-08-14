using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f; // Time before the bullet is destroyed
    private Transform player;

    void Start()
    {
        // Destroy the bullet after a certain time to avoid memory leaks
        Destroy(gameObject, lifetime);

        // Find the player
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Calculate the direction to the player
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }

    void Update()
    {
        // Remove the translation logic from Update as we now use Rigidbody2D for movement
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Add your collision logic here
        if (collision.CompareTag("Player"))
        {
            // Damage the player (implement your own damage logic)
            Debug.Log("Player hit!");

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}

