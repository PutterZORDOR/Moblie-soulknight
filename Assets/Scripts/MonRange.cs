using UnityEngine;

public class MonsterRange : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    public float moveSpeed = 5f; // Speed of the monster
    public float attackRange = 1.5f; // Range within which the monster can attack
    public float shootRange = 5f; // Range within which the monster can shoot
    public float attackCooldown = 2f; // Cooldown time between attacks
    public float detectionRange = 10f; // Range within which the monster can detect the player
    public GameObject bulletPrefab; // Bullet prefab
    public Transform shootPoint; // Point from where bullets are shot

    private float lastAttackTime;
    private bool playerInRange = false;
    private bool playerInShootRange = false;

    void Start()
    {
        // Find the player object in the scene by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set up detection range using a circle collider
        CircleCollider2D detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
    }

    void Update()
    {
        if (playerInRange)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= shootRange)
            {
                playerInShootRange = true;
                ShootPlayer();
            }
            else
            {
                playerInShootRange = false;
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the monster in the direction of the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void ShootPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Implement your shooting logic here
            Debug.Log("Monster shoots at the player!");
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left detection range!");
            playerInRange = false;
            playerInShootRange = false;
        }
    }
}
