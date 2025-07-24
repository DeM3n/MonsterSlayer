using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;
    private int jumpCount;

    [Header("Rolling (Dash)")]
    public float rollForce = 10f;
    public float rollCooldown = 1f;
    private float lastRollTime;
    private bool isRolling = false;

    [Header("Ladder")]
    private bool isClimbing = false;
    private bool inLadderZone = false;
    public float climbSpeed = 4f;
    private float verticalInput;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private float horizontalInput;
    private Rigidbody2D rb;
    private Animator animator;
    private float originalGravity;

    public bool canMove = true;
    private bool isAttacking = false;

    [Header("Heavy Attack Settings")]
    public float heavyAttackCooldown = 1.0f;  // Cooldown time in seconds
    private float lastHeavyAttackTime = -99f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        if (!canMove) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (inLadderZone && !isClimbing && Mathf.Abs(verticalInput) > 0.1f)
        {
            StartClimbing();
        }
        if (isClimbing && (!inLadderZone || isGrounded))
        {
            StopClimbing(true);
        }

        HandleAttack();
        HandleJump();
        HandleRoll();
        HandleAnimations();
        FlipCharacter();

        // Cho phép attack tiếp khi anim kết thúc
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (isAttacking &&
            !state.IsName("Attack01") && !state.IsName("Attack02") && !state.IsName("JumpAttack"))
        {
            isAttacking = false;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpCount = 0;

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
                animator.speed = 1f;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.speed = 1f;
            }
            return;
        }
        else
        {
            rb.gravityScale = originalGravity;
            animator.speed = 1f;
        }

        if (isRolling)
        {
            return;
        }

        if (isAttacking)
        {
            if (isGrounded)
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            jumpCount++;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");

            if (isClimbing || inLadderZone)
            {
                StopClimbing(true);
                float pushDir = Mathf.Sign(transform.localScale.x);
                rb.linearVelocity = new Vector2(pushDir * 2f, jumpForce);
            }
        }
    }

    void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastRollTime > rollCooldown && isGrounded && !inLadderZone && !isAttacking && !isRolling)
        {
            isRolling = true;
            isAttacking = true;
            float rollDir = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(rollDir * rollForce, 0), ForceMode2D.Impulse);
            animator.SetTrigger("Roll");
            lastRollTime = Time.time;
            Invoke(nameof(EndRoll), 0.4f);
        }
    }
    void EndRoll()
    {
        isRolling = false;
        isAttacking = false;
    }

    void HandleAttack()
    {
        if (isClimbing || isAttacking || isRolling) return;

        // Attack01: Chuột trái, không cooldown
        if (Input.GetMouseButtonDown(0))
        {
            animator.ResetTrigger("Attack02");
            animator.SetTrigger("Attack01");
            isAttacking = true;
        }
        // Attack02: Chuột phải, cooldown
        else if (Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastHeavyAttackTime >= heavyAttackCooldown)
            {
                animator.ResetTrigger("Attack01");
                animator.SetTrigger("Attack02");
                isAttacking = true;
                lastHeavyAttackTime = Time.time;
            }
        }
    }

    void HandleAnimations()
    {
        animator.SetBool("isClimbing", isClimbing);
        animator.SetBool("isRunning", !isClimbing && horizontalInput != 0 && !isAttacking && !isRolling);
        animator.SetBool("isJumping", !isGrounded && !isClimbing);
        animator.SetBool("isFalling", !isGrounded && rb.linearVelocity.y < 0 && !isClimbing);
    }

    void FlipCharacter()
    {
        if (!isClimbing && horizontalInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(horizontalInput) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.linearVelocity = Vector2.zero;
        animator.Play("Climb", 0, 0f);
        animator.speed = 1f;
    }

    void StopClimbing(bool exited)
    {
        isClimbing = false;
        if (exited)
            rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        animator.speed = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            inLadderZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            inLadderZone = false;
            StopClimbing(true);
        }
    }
}
