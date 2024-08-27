using UnityEngine;

public class ShortGun_Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float maxDistance = 4f;
    private Vector2 startPos;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = rb.position;
        rb.velocity = transform.right * speed;
    }

    void Update()
    {
        // Check the distance the bullet has traveled
        float distanceTravelled = Vector2.Distance(startPos, rb.position);
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the bullet on collision with any object
        Destroy(gameObject);
    }
}
