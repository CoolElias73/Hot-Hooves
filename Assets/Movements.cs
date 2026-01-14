using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float groundDistance = 0.6f;

    private Rigidbody2D rb;
    private float horizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            horizontal =
                (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1 : 0) +
                (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0);

            if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundDistance);
    }
}
