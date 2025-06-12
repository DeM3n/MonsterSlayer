using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float moveInput;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    private int jumpCount;
    private bool isGrounded;

    [Header("Rolling / Dashing")]
    public float rollForce = 12f;
    public float rollCooldown = 1f;
    private bool canRoll = true;
    private float rollTimer;

    [Header("Attack")]
    public float attackDuration = 0.3f;
    private bool isAttacking;

    [Header("Crouch/Drop Platform")]
    public float crouchFallDelay = 0.2f; // time to ignore platform
    private Collider2D playerCollider;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        // FRAME: Horizontal Movement Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // FRAME: Flip Character Sprite
        HandleFlip();

        // FRAME: Check if Grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpCount = maxJumps;

        // FRAME: Jump with Space or W
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }

        // FRAME: Roll / Dash with Shift
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll)
        {
            StartRoll();
        }

        // FRAME: Attack with Z
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }

        // FRAME: Drop Through Platform with S or Shift
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftShift)) && isGrounded)
        {
            StartCoroutine(DropThroughPlatform());
        }

        // FRAME: Roll Cooldown Timer
        if (!canRoll)
        {
            rollTimer += Time.deltaTime;
            if (rollTimer >= rollCooldown)
            {
                canRoll = true;
                rollTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // FRAME: Apply Horizontal Movement
        if (!isAttacking)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    // FUNCTION: Flip Character Sprite Based on Direction
    void HandleFlip()
    {
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // FUNCTION: Dash / Roll Behavior
    void StartRoll()
    {
        canRoll = false;
        float direction = facingRight ? 1 : -1;
        rb.velocity = new Vector2(direction * rollForce, rb.velocity.y);
    }

    // FUNCTION: Simulate Melee Attack Behavior
    System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;
        Debug.Log("Attack Start");
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        Debug.Log("Attack End");
    }

    // FUNCTION: Drop Through Platform (One Way)
    System.Collections.IEnumerator DropThroughPlatform()
    {
        // Temporarily disable player's collision with platform
        PlatformEffector2D[] platforms = FindObjectsOfType<PlatformEffector2D>();
        foreach (PlatformEffector2D platform in platforms)
        {
            Physics2D.IgnoreCollision(playerCollider, platform.GetComponent<Collider2D>(), true);
        }

        yield return new WaitForSeconds(crouchFallDelay);

        foreach (PlatformEffector2D platform in platforms)
        {
            Physics2D.IgnoreCollision(playerCollider, platform.GetComponent<Collider2D>(), false);
        }
    }

    // DEBUG: Draw Ground Check Gizmos
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
