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
    private Ladder currentLadder;

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

        HandleAttack();
        HandleJump();
        HandleRoll();
        HandleAnimations();
        FlipCharacter();

        if (inLadderZone && !isClimbing && Mathf.Abs(verticalInput) > 0.1f)
        {
            StartClimbing();
        }

        if (isClimbing)
        {
            if (isGrounded)
            {
                StopClimbing(true);
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove || isAttacking) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpCount = 0;

        if (isClimbing && currentLadder != null)
        {
            float newY = transform.position.y + verticalInput * climbSpeed * Time.fixedDeltaTime;
            float bottomY = currentLadder.bottomPoint.position.y;
            newY = Mathf.Max(newY, bottomY);
            rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);
            rb.MovePosition(new Vector2(transform.position.x, newY));
        }
        else
        {
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
            Invoke(nameof(EndAttack), 0.4f); // giữ nhân vật đứng yên trong thời gian animation roll
        }
    }


    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isClimbing && !isAttacking)
        {
            if (Time.time - lastAttackTime > comboResetTime)
            {
                attackIndex = 0;
            }

            attackIndex = Mathf.Clamp(attackIndex + 1, 1, 2); // dùng Attack1 và Attack2
            lastAttackTime = Time.time;
            isAttacking = true;

            if (!isGrounded)
            {
                animator.SetTrigger("JumpAttack");
                Debug.Log("JumpAttack");
            }
            else
            {
                animator.SetTrigger("Attack" + attackIndex);
                Debug.Log("Attack: " + attackIndex);
            }

            Invoke(nameof(EndAttack), 0.4f);
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
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    void StopClimbing(bool exited)
    {
        isClimbing = false;

        if (exited)
        {
            rb.gravityScale = originalGravity;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            Ladder ladder = other.GetComponentInParent<Ladder>();
            if (ladder != null)
            {
                inLadderZone = true;
                currentLadder = ladder;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            inLadderZone = false;
            StopClimbing(true);
            currentLadder = null;
        }
    }
}
