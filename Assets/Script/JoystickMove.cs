using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    public bool isRight = true;

    public Transform weaponTransform;
    public float detectionRange = 5f;

    [SerializeField] private bool enemyDetected = false;
    [SerializeField] private bool flipEnabled = true;

    private Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        enemyDetected = DetectEnemy();

        moveDirection = movementJoystick.Direction;
        MoveCharacter(moveDirection);

        if (!enemyDetected && flipEnabled)
        {
            FlipCharacter(moveDirection);
        }

        RotateWeapon(moveDirection);
    }

    private void MoveCharacter(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * playerSpeed, direction.y * playerSpeed);
    }

    public void FlipCharacter(Vector2 direction)
    {
        if (direction.x > 0 && !isRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void RotateWeapon(Vector2 direction)
    {
        if (!enemyDetected && direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private bool DetectEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= detectionRange)
            {
                return true;
            }
        }
        return false;
    }

    public void DisableFlip()
    {
        flipEnabled = false;
    }

    public void EnableFlip()
    {
        flipEnabled = true;
    }

    public void FlipCharacterBasedOnDirection()
    {
        // Ensure the character faces the direction based on movement.
        FlipCharacter(movementJoystick.Direction);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
