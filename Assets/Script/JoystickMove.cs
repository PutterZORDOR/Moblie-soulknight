using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    private Animator PlayerAnim;
    public bool isRight = true;

    [SerializeField] private bool flipEnabled = true;

    private Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        moveDirection = movementJoystick.Direction;
        MoveCharacter(moveDirection);

        if (flipEnabled)
        {
            FlipCharacter(moveDirection);
        }
    }

    private void MoveCharacter(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * playerSpeed, direction.y * playerSpeed);
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
}
