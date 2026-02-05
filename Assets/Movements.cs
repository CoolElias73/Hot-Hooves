using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 25f;
    public float doubleJumpForce = 22f;
    public float groundDistance = 1.1f;
    public float groundAcceleration = 1000f;
    public float groundDeceleration = 1000f;
    public float iceAcceleration = 30f;
    public float iceDeceleration = 5f;
    public float airAcceleration = 600f;
    public float airDeceleration = 800f;
    public Vector3 scale;
    public float coyoteTime = 0.15f;
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public float normalJumpPitch = 1f;
    public float doubleJumpPitch = 1.5f;

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
                    SetJumpPitch(normalJumpPitch);
                    PlayJumpSound();
                }
                else if (canDoubleJump)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                    canDoubleJump = false;
                    SetJumpPitch(doubleJumpPitch);
                    PlayJumpSound();
                }
            }

        }
        transform.localScale = scale;
    }

    void FixedUpdate()
    {
        float targetSpeed = horizontal * speed;
        bool grounded = IsGrounded();
        bool onIce = grounded && IsOnIce();
        float accel = 0f;

        if (grounded)
        {
            accel = horizontal != 0
                ? (onIce ? iceAcceleration : groundAcceleration)
                : (onIce ? iceDeceleration : groundDeceleration);
        }
        else
        {
            accel = horizontal != 0 ? airAcceleration : airDeceleration;
        }

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    bool IsGrounded()
    {
        return TryGetGroundHit(out _);
    }

    bool IsOnIce()
    {
        if (!TryGetGroundHit(out RaycastHit2D hit))
            return false;

        return hit.collider != null && hit.collider.GetComponent<IceSurface>() != null;
    }

    bool TryGetGroundHit(out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(rightPoint.position, Vector2.down, groundDistance, groundMask);
        if (hit.collider != null)
            return true;

        hit = Physics2D.Raycast(leftPoint.position, Vector2.down, groundDistance, groundMask);
        return hit.collider != null;
    }

    void PlayJumpSound()
    {
        if (audioSource != null && jumpClip != null)
            audioSource.PlayOneShot(jumpClip);
    }

    void SetJumpPitch(float pitch)
    {
        if (audioSource != null)
            audioSource.pitch = pitch;
    }

    public void IncreaseStats(float speedIncrease, float jumpIncrease)
    {
        speed += speedIncrease;
        jumpForce += jumpIncrease;
    }
}
