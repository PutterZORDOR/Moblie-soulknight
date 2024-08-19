using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    private bool isRight = true;

    public Transform weaponTransform;
    public float detectionRange = 5f;  // Range to detect enemies

    private bool enemyDetected = false; // Track whether an enemy is detected
    private bool flipEnabled = true;    // Control flip state

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if an enemy is detected
        enemyDetected = DetectEnemy();

        // รับค่าจากจอยสติ๊ก
        Vector2 moveDirection = movementJoystick.Direction;
        MoveCharacter(moveDirection);

        // Only flip character if no enemy is detected and flip is enabled
        if (!enemyDetected && flipEnabled)
        {
            FlipCharacter(moveDirection);
        }

        RotateWeapon(moveDirection);
    }

    private void MoveCharacter(Vector2 direction)
    {
        // เคลื่อนที่ตัวละคร
        rb.velocity = new Vector2(direction.x * playerSpeed, direction.y * playerSpeed);
    }

    private void FlipCharacter(Vector2 direction)
    {
        if (direction.x > 0 && isRight)
        {
            Flip();
        }
        else if (direction.x < 0 && !isRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // พลิกหน้าตัวละคร
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void RotateWeapon(Vector2 direction)
    {
        // ถ้าไม่มีศัตรูในระยะ ให้ควบคุมทิศทางของอาวุธด้วยจอยสติ๊ก
        if (!enemyDetected && direction != Vector2.zero)
        {
            // คำนวณมุมของทิศทาง
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private bool DetectEnemy()
    {
        // หาenemyจากแท็กของobj
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= detectionRange)
            {
                return true; // Enemy detected within range
            }
        }
        return false; // No enemy detected
    }

    public void DisableFlip()
    {
        flipEnabled = false;
    }

    public void EnableFlip()
    {
        flipEnabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงระยะตรวจจับศัตรูในโหมด Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
