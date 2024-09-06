using UnityEngine;

public class Meteor : MonoBehaviour
{
    public int damage = 50; // The damage dealt by the meteor

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
                Debug.Log("Meteor hit the player and dealt " + damage + " damage.");
            }

            // Destroy the meteor after it hits the player
            gameObject.SetActive(false);
        }
    }
}
