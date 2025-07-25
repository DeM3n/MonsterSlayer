using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator animator;

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
    private float originalGravity;
    public bool canMove = true;
    private bool isAttacking = false;

    [Header("Attack")]
    public float heavyAttackCooldown = 1f;
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

        // Climb check
        if (inLadderZone && !isClimbing && Mathf.Abs(verticalInput) > 0.1f)
        {
            StartClimbing();
        }
        if (isClimbing && !inLadderZone)
        {
            StopClimbing(true);
        }

        HandleAttack();
        HandleJump();
        HandleRoll();
        HandleAnimations();
        FlipCharacter();

        // Unset attack state after anim
        if (isAttacking)
        {
            AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
            if (!animState.IsName("Attack01") && !animState.IsName("Attack02") && !animState.IsName("JumpAttack"))
                isAttacking = false;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);

            // Freeze climb animation when không bấm lên/xuống
            animator.speed = Mathf.Abs(verticalInput) > 0.05f ? 1f : 0f;
            return;
        }
        else
        {
            rb.gravityScale = originalGravity;
            animator.speed = 1f;
        }

        if (isRolling)
            return;

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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
        if (isClimbing && Input.GetKeyDown(KeyCode.Space))
        {
            StopClimbing(true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastRollTime > rollCooldown && isGrounded && !isClimbing && !isAttacking && !isRolling)
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

        // Attack01 (Normal)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack01");
            isAttacking = true;
        }

        // Attack02 (Heavy/Combo)
        if (Input.GetMouseButtonDown(1) && Time.time - lastHeavyAttackTime >= heavyAttackCooldown)
        {
            animator.SetTrigger("Attack02");
            isAttacking = true;
            lastHeavyAttackTime = Time.time;
        }
    }

    void HandleAnimations()
    {
        animator.SetBool("isClimbing", isClimbing);
        animator.SetBool("isRunning", !isClimbing && Mathf.Abs(horizontalInput) > 0.1f && !isAttacking && !isRolling);
        animator.SetBool("isJumping", !isGrounded && !isClimbing && rb.linearVelocity.y > 0.01f);
        animator.SetBool("isFalling", !isGrounded && rb.linearVelocity.y < -0.01f && !isClimbing);
    }

    void FlipCharacter()
    {
        if (!isClimbing && Mathf.Abs(horizontalInput) > 0.01f)
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
        if (other.CompareTag("LadderArea"))
        {
            inLadderZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LadderArea"))
        {
            inLadderZone = false;
            StopClimbing(true);
        }
    }
}
