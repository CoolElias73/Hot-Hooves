using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float groundDistance = 0.6f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontal;
    private bool canDoubleJump = true;

    public LayerMask groundMask;

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

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (IsGrounded())
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    print("1");
                }
                else if (canDoubleJump)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    print("2");
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
        return Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
    }
}