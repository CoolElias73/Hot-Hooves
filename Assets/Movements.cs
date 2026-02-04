using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMovement : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 25f;
    public float groundDistance = 1.1f;
    public Vector3 scale;
    public float coyoteTime = 0.15f;
    public AudioSource audioSource;
    public AudioClip jumpClip;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontal;
    private bool canDoubleJump = true;
    private float coyoteTimeCounter;

    public LayerMask groundMask;
    public Transform rightPoint;
    public Transform leftPoint;
    
    void Awake()
    {
        scale = new Vector3(1, 1, 1);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            horizontal =
                (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1 : 0) +
                (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0);

            if (horizontal < 0)
                scale.x = -1;
            else if (horizontal > 0)
                scale.x = 1;

            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
                canDoubleJump = true;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (coyoteTimeCounter > 0f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    coyoteTimeCounter = 0f;
                    PlayJumpSound();
                }
                else if (canDoubleJump)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    canDoubleJump = false;
                    PlayJumpSound();
                }
            }

        }
        transform.localScale = scale;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(rightPoint.position, Vector2.down, groundDistance, groundMask) || Physics2D.Raycast(leftPoint.position, Vector2.down, groundDistance, groundMask);
    }

    void PlayJumpSound()
    {
        if (audioSource != null && jumpClip != null)
            audioSource.PlayOneShot(jumpClip);
    }

    public void IncreaseStats(float speedAmount, float jumpAmount)
    {
        speed += speedAmount;
        jumpForce += jumpAmount;
    }

}
