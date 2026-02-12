using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 33f;
    public float doubleJumpForce = 22f;
    public float groundDistance = 1.1f;
    public float iceAcceleration = 30f;
    public float iceDeceleration = 5f;
    public float iceSpeedDrag = 0.2f;
    public float iceTurnAcceleration = 8f;
    public Vector3 scale;
    public float coyoteTime = 0.15f;
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public float normalJumpPitch = 1f;
    public float doubleJumpPitch = 1.5f;
    public float jumpForceAdded;
    public Sprite goatCitronStanding;
    public bool noclipFly = false;
    public float flySpeed = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float horizontal;
    private bool canDoubleJump = true;
    private float coyoteTimeCounter;
    private Sprite defaultSprite;
    private bool _noclipActive;
    private float _savedGravity;
    private bool _savedColliderEnabled;
    private Collider2D _playerCollider;

    public LayerMask groundMask;
    public Transform rightPoint;
    public Transform leftPoint;
    
    void Awake()
    {
        scale = new Vector3(1, 1, 1);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<Collider2D>();
        defaultSprite = sr.sprite;
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

            if (Keyboard.current.vKey.wasPressedThisFrame)
                noclipFly = !noclipFly;

            if (horizontal < 0)
                scale.x = -1;
            else if (horizontal > 0)
                scale.x = 1;

            UpdateNoclipState();

            bool grounded = IsGrounded();
            if (grounded)
            {
                coyoteTimeCounter = coyoteTime;
                canDoubleJump = true;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (!grounded && goatCitronStanding != null)
            {
                sr.sprite = goatCitronStanding;
            }
            else if (grounded && sr.sprite == goatCitronStanding)
            {
                sr.sprite = defaultSprite;
            }

            if (!_noclipActive && Keyboard.current.spaceKey.wasPressedThisFrame)
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
        if (_noclipActive)
        {
            float vertical =
                (Keyboard.current != null && (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) ? 1 : 0) +
                (Keyboard.current != null && (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) ? -1 : 0);

            bool boost = Keyboard.current != null &&
                (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed);
            float moveSpeed = (flySpeed > 0f ? flySpeed : speed) * (boost ? 2f : 1f);
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
            return;
        }

        bool onIce = IsOnIce();
        if (onIce)
        {
            float targetSpeed = horizontal * speed;
            float accel = horizontal != 0 ? iceAcceleration : iceDeceleration;
            if (horizontal != 0 && Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(targetSpeed) && Mathf.Abs(rb.linearVelocity.x) > 0.01f)
                accel = iceTurnAcceleration;
            float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.fixedDeltaTime);
            float speedDrag = Mathf.Abs(newX) * iceSpeedDrag * Time.fixedDeltaTime;
            if (horizontal == 0)
                newX = Mathf.MoveTowards(newX, 0f, speedDrag);
            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
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

    void UpdateNoclipState()
    {
        if (noclipFly && !_noclipActive)
        {
            _noclipActive = true;
            _savedGravity = rb.gravityScale;
            _savedColliderEnabled = _playerCollider != null ? _playerCollider.enabled : false;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            if (_playerCollider != null)
                _playerCollider.enabled = false;
        }
        else if (!noclipFly && _noclipActive)
        {
            _noclipActive = false;
            rb.gravityScale = _savedGravity;
            if (_playerCollider != null)
                _playerCollider.enabled = _savedColliderEnabled;
        }
    }

    public void IncreaseStats(float speedIncrease, float jumpIncrease)
    {
        speed += speedIncrease;
        jumpForce += jumpIncrease;
        jumpForceAdded += jumpIncrease;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            collision.gameObject.GetComponent<ImpactShake>()?.Shake();
        }
    }
   

    

}
