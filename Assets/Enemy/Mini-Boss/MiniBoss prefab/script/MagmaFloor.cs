using UnityEngine;
using System.Collections;

public class MagmaFloor : MonoBehaviour
{
    public float damagePerTick = 20f; // Damage dealt while on the magma floor
    public float burnDamage = 2f; // Damage dealt after leaving the magma floor
    public int burnTicks = 10; // Number of times burn damage is applied
    public float damageInterval = 1.5f; // Interval between damage ticks while on the magma floor
    public float burnInterval = 1f; // Interval between burn damage ticks
    private bool playerOnMagma = false;
    private Coroutine burnCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnMagma = true;
            StartCoroutine(DamagePlayerOverTime(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnMagma = false;

            if (burnCoroutine != null)
            {
                StopCoroutine(burnCoroutine);
            }
            burnCoroutine = StartCoroutine(ApplyBurnDamage(collision.gameObject));
        }
    }

    private IEnumerator DamagePlayerOverTime(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        while (playerOnMagma && playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private IEnumerator ApplyBurnDamage(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        for (int i = 0; i < burnTicks; i++)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((float)burnDamage); // Here, burnDamage is a float
            }
            yield return new WaitForSeconds(burnInterval); // Ensure burnInterval is a float
        }
    }

}
