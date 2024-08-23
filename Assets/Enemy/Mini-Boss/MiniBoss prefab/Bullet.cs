using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    public float lifetime = 2f; // Lifetime of the bullet
    private Vector3 direction; // Direction of the bullet

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the bullet after its lifetime expires
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
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
            // Implement damage logic here
            Debug.Log("Bullet hit the player!");
            Destroy(gameObject); // Destroy the bullet upon hitting the player
        }
        
    }
}
