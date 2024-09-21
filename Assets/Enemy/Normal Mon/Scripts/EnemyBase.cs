using System.Collections;
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
    private SpriteRenderer spriteRenderer;
    public float blinkTime;
    public Color blinkColor;

    protected Collider2D col_Player;
    protected Collider2D col_Enemy;

    protected bool isDash;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // ใช้ ?. เพื่อป้องกันการเรียก position เมื่อ player เป็น null
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Initialize current health

        col_Enemy = GetComponent<Collider2D>();
        if (player != null)
        {
            col_Player = player.GetComponent<Collider2D>();
        }
    }

    protected void DetectPlayer()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange && !isDie)
        {
            OnPlayerDetected();
            if (!isDash)
            {
                FlipTowardsPlayer();
            }
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        DetectPlayer();
    }
    // Method to be overridden in child classes for specific behaviors
    protected abstract void OnPlayerDetected();

    // Method to take damage
    public virtual void TakeDamage(int damage)
    {
        if (!isDie)
        {
            currentHealth -= damage;
            StartCoroutine(Timer());

            if (currentHealth <= 0)
            {
                isDie = true;
                Mana_Manager manaManager = GameObject.FindGameObjectWithTag("Mana Pool").GetComponent<Mana_Manager>();
                foreach(GameObject mana in manaManager.ManaPool)
                {
                    if (!mana.activeSelf)
                    {
                        mana.transform.position = transform.position;
                        mana.SetActive(true);
                        break;
                    }
                }
                DungeonSystem.instance.AllEnermyInRoom--;
                OnDefeated();
            }
        }
    }
    public IEnumerator Timer()
    {
        spriteRenderer.color = blinkColor;
        yield return new WaitForSeconds(blinkTime);
        spriteRenderer.color = Color.white;
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
