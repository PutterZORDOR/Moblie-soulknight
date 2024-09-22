using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    private Animator PlayerAnim;
    public bool isRight = true;

    [SerializeField] private bool flipEnabled = true;
    public Transform weaponTransform;
    public Weapon weapon;

    private Vector2 moveDirection;
    public bool enableRotateWeapon;
    public SpriteRenderer spriteRenderer;

    public bool canDash = true;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.5f;
    public float dashingCooldown = 1f;

    private Coroutine slowCoroutine = null;
    private float originalSpeed;
    private float originalDashingPower;

    public InputAction DashButton;
    [SerializeField] private TrailRenderer tr;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
        originalSpeed = playerSpeed;
        originalDashingPower = dashingPower;
        DashButton.Enable();
    }

    private void Update()
    {
        moveDirection = movementJoystick.Direction;
        if (!isDashing)
        {
            MoveCharacter(moveDirection);
        }
        if (moveDirection != Vector2.zero && !isDashing)
        {
            PlayerAnim.SetBool("isMoving", true);
        }
        else
        {
            PlayerAnim.SetBool("isMoving", false);
        }

        if (flipEnabled)
        {
            FlipCharacter(moveDirection);
        }
        if (enableRotateWeapon && weapon.isInWeaponSlot && weapon.gameObject.layer != LayerMask.NameToLayer("Sword"))
        {
            if (weapon.tag == "Equipped")
                RotateWeapon(moveDirection);
        }

        if (isDashing)
        {
            return;
        }

        if (DashButton.triggered && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isDashing)
        {
            MoveCharacter(moveDirection);
        }
    }

    private void MoveCharacter(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * playerSpeed, direction.y * playerSpeed);
    }
    public void RotateWeapon(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Normalize the angle to be between -180 and 180 degrees
            angle = Mathf.Repeat(angle + 360, 360);
            if (angle > 180) angle -= 360;

            // Check if the angle is within the desired range
            if (Mathf.Abs(angle) <= 90)
            {
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else if (Mathf.Abs(angle) > 90)
            {
                weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = true;
            }
        }
    }


    public void FlipCharacter(Vector2 direction)
    {
        if (direction.x > 0 && !isRight)
        {
            Flip(-2);  // Flip to the right
        }
        else if (direction.x < 0 && isRight)
        {
            Flip(2); // Flip to the left
        }
    }

    private void Flip(float newScaleX)
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x = newScaleX;  // Set scale to 2 or -2
        transform.localScale = scale;
    }
    public Vector2 GetMovementDirection()
    {
        return moveDirection; // Return the current movement direction
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
    public void Debuff_Slow(float delay)
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        slowCoroutine = StartCoroutine(SlowEffect(delay));
    }
    private IEnumerator SlowEffect(float delay)
    {
        playerSpeed = originalSpeed * 0.5f;
        dashingPower = originalDashingPower * 0.5f;

        yield return new WaitForSeconds(delay);

        playerSpeed = originalSpeed;
        dashingPower = originalDashingPower;

        slowCoroutine = null;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashDirection = moveDirection != Vector2.zero ? moveDirection.normalized : (isRight ? Vector2.right : Vector2.left);

        rb.velocity = dashDirection * dashingPower;

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;

        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}