using UnityEngine;

public class MageEnemy : EnemyBase
{
    public GameObject magicOrbPrefab; // Assign the magic orb prefab in the inspector
    public Transform firePoint; // Assign the fire point in the inspector
    public float summonCooldown = 5f; // Time between summoning orbs
    public float orbSpeed = 5f; // Speed of the homing orb
    public float orbLifetime = 10f; // Lifetime of the orb
    public int orbDamage = 30;
    public Animator anim;

    private float lastSummonTime;
    private bool isAttacking = false; // ตัวแปรสำหรับเช็คสถานะการโจมตี

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        moveSpeed = 1.5f; // Mage moves slower but has powerful magic attacks
    }

    protected override void Update()
    {
        base.Update();

        // ถ้าโจมตีอยู่ ให้หยุดเคลื่อนไหว
        if (isAttacking)
        {
            anim.SetBool("isWalking", false); // หยุดแอนิเมชันการเดินขณะโจมตี
            return; // ออกจากฟังก์ชัน Update เพื่อหยุดการเคลื่อนไหว
        }

        // ตรวจสอบว่าเวลาที่จะเรียกใช้งาน Magic Orb ถึงหรือยัง
        if (Time.time >= lastSummonTime + summonCooldown)
        {
            SummonMagicOrb();
            lastSummonTime = Time.time;
        }
    }

    protected override void OnPlayerDetected()
    {
        if (playerDetected && !isAttacking) // ถ้าผู้เล่นถูกตรวจจับและไม่ได้โจมตี
        {
            MoveTowardsPlayer(); // เคลื่อนที่ไปหาผู้เล่น
        }
        else
        {
            anim.SetBool("isWalking", false); // หยุดการเคลื่อนไหวถ้าไม่เจอผู้เล่น
        }
    }

    private void SummonMagicOrb()
    {
        isAttacking = true; // กำลังโจมตีอยู่

        anim.SetTrigger("Attack"); // เล่นแอนิเมชันโจมตี

        // สร้างลูกไฟ
        GameObject orb = Instantiate(magicOrbPrefab, firePoint.position, Quaternion.identity);
        HomingOrb homingOrbScript = orb.GetComponent<HomingOrb>();

        if (homingOrbScript != null)
        {
            homingOrbScript.SetTarget(player);
            homingOrbScript.SetSpeed(orbSpeed);
            homingOrbScript.SetDamage(orbDamage);
            Destroy(orb, orbLifetime); // ทำลายลูกไฟหลังจากครบเวลา
        }

        Debug.Log("Mage summoned a homing magic orb!");
    }

    // ฟังก์ชันที่จะถูกเรียกเมื่อแอนิเมชันการโจมตีเสร็จสิ้น
    public void OnAttackComplete()
    {
        isAttacking = false; // การโจมตีเสร็จสิ้นแล้ว
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // เช็คว่าตำแหน่งใหม่แตกต่างจากตำแหน่งปัจจุบันหรือไม่
        if (newPosition != (Vector2)transform.position)
        {
            anim.SetBool("isWalking", true); // เล่นแอนิเมชันเดิน
        }
        else
        {
            anim.SetBool("isWalking", false); // หยุดแอนิเมชันเดิน
        }

        transform.position = newPosition;
    }
}
