using UnityEngine;

public class Movement : MonoBehaviour
{
    public Joystick movement;
    public float speed;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
       if(movement.Direction.y !=0)
        {
            rb.velocity = new Vector2(movement.Direction.x * speed, movement.Direction.y * speed);
        }
       else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
