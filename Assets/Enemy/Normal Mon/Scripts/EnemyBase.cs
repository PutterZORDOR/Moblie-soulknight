using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected Transform player;
    public float detectionRange = 10f;
    public float moveSpeed;
    public bool playerDetected;

    public int maxHealth = 100; // Maximum health for the enemy
    [SerializeField]protected int currentHealth; // Current health of the enemy

    private bool facingRight = true; // ตัวแปรเพื่อเช็คว่าศัตรูกำลังหันหน้าไปทางขวาหรือไม่
    protected bool isDie;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth; // Initialize current health
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectPlayer();
    }

    protected void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange && !isDie)
        {
            OnPlayerDetected();
            FlipTowardsPlayer(); // เพิ่มการ Flip ตามทิศทางผู้เล่น
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
    }

    // Method to be overridden in child classes for specific behaviors
    protected abstract void OnPlayerDetected();

    // Method to take damage
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage!");

        if (currentHealth <= 0)
        {
            isDie = true;
            OnDefeated();
        }
    }

    // Method to handle when the enemy is defeated
    protected virtual void OnDefeated()
    {
        Debug.Log($"{gameObject.name} defeated!");
        Destroy(gameObject);
    }

    // ฟังก์ชันสำหรับ Flip ศัตรูไปทางผู้เล่น
    protected void FlipTowardsPlayer()
    {
        // เช็คว่าผู้เล่นอยู่ทางซ้ายหรือทางขวาของศัตรู
        if ((player.position.x < transform.position.x && facingRight) ||
            (player.position.x > transform.position.x && !facingRight))
        {
            // พลิกตัวศัตรู
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // เปลี่ยนทิศทางการหันหน้า
            transform.localScale = localScale;
        }
    }
}
