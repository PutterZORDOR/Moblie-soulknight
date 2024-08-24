using UnityEngine;

public class ShortGun_Bullet : MonoBehaviour
{
    public float maxDistance = 4f; // Maximum distance the pellet can travel
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position; // Record the starting position of the bullet
    }

    void Update()
    {
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject); // Destroy the pellet after it has traveled the max distance
        }
    }
}
