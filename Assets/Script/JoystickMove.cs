using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    private Animator PlayerAnim;
    public bool isRight = true;

    public Transform weaponTransform;
    public float detectionRange = 5f;

    [SerializeField] private bool enemyDetected = false;
    [SerializeField] private bool flipEnabled = true;

    private Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>(); // Initialize Animator
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
            if (weaponTransform.gameObject.layer == LayerMask.NameToLayer("Gun"))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }

    private bool DetectEnemy()
    {
        if (weaponTransform.gameObject.layer == LayerMask.NameToLayer("Gun"))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, LayerMask.GetMask("Enemy"));
            return hitColliders.Length > 0;
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
        FlipCharacter(movementJoystick.Direction);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
