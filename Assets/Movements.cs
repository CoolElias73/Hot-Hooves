using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 13f;
    public float groundDistance = 1.1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontal;
    private bool canDoubleJump = true;

    public LayerMask groundMask;
    public Transform rightPoint;
    public Transform leftPoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            horizontal =
                (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1 : 0) +
                (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0);

            if (horizontal < 0)
                sr.flipX = true;
            else if (horizontal > 0)
                sr.flipX = false;

            if (IsGrounded())
            {
                canDoubleJump = true;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (IsGrounded())
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                }
                else if (canDoubleJump)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    canDoubleJump = false;
                }
            }
            
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(rightPoint.position, Vector2.down, groundDistance, groundMask) || Physics2D.Raycast(leftPoint.position, Vector2.down, groundDistance, groundMask);
    }
}