using UnityEngine;

public class Mana : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public float moveSpeed = 3.0f;
    public int coin;
    public int Mana_Pure;

    private Transform playerTransform;

    public void SetCoin(int coins)
    {
        coin = coins;
    }
    public void SetMana(int mana)
    {
        Mana_Pure = mana;
    }
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (IsPlayerInRange())
        {
            MoveTowardsPlayer();
        }
    }
    bool IsPlayerInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                playerTransform = hitCollider.transform;
                return true;
            }
        }
        return false;
    }
    void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.instance.RecoverMana(Mana_Pure);
            CoinManager.instance.AddCoins(coin);
            gameObject.SetActive(false);
        }
    }
}

