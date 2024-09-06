using UnityEngine;

public class SageBullet : MonoBehaviour
{
    public int damage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
                Debug.Log("Sage hit the player and dealt " + damage + " damage.");
            }

            // Instead of destroying the bullet, deactivate it
            gameObject.SetActive(false);
        }
    }

}
