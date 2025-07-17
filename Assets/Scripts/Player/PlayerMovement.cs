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

    [Header("Attack")]
    public float comboResetTime = 1f;
    private int attackIndex = 0;
    private float lastAttackTime;
    private float nextAttackTime = 0f;

    private float climbAnimTime = 0f;

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

        if (inLadderZone && Mathf.Abs(verticalInput) > 0.1f)
        {
            if (!isClimbing) StartClimbing();
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
    }

    void FixedUpdate()
    {
        if (!canMove || isAttacking) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpCount = 0;

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackIndex = 0;
        }
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastRollTime > rollCooldown && isGrounded && !inLadderZone && !isAttacking)
        {
            isAttacking = true;
            float rollDir = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(rollDir * rollForce, 0), ForceMode2D.Impulse);
            animator.SetTrigger("Roll");
            lastRollTime = Time.time;
            Invoke(nameof(EndAttack), 0.4f);
        }
    }

    void HandleAttack()
    {
        if (Time.time < nextAttackTime || isClimbing || isAttacking) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttackTime > comboResetTime)
                attackIndex = 0;

            attackIndex = Mathf.Clamp(attackIndex + 1, 1, 2);
            lastAttackTime = Time.time;
            nextAttackTime = Time.time + 1f;
            isAttacking = true;

            if (!isGrounded)
                animator.SetTrigger("JumpAttack");
            else
                animator.SetTrigger("Attack" + attackIndex);

            Invoke(nameof(EndAttack), 1f);
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    void HandleAnimations()
    {
        animator.SetBool("isRunning", !isClimbing && horizontalInput != 0);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetBool("isJumping", !isGrounded && !isClimbing);

        animator.SetFloat("ClimbSpeed", isClimbing ? Mathf.Abs(verticalInput) : 0f);
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
        animator.Play("Climb", 0, climbAnimTime);
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
