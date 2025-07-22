using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;
    private int jumpCount;

    [Header("Dash (Roll)")]
    public float rollForce = 12f;
    public float rollCooldown = 1f;
    public float rollDuration = 0.4f;
    private float lastRollTime;

    [Header("Attack Combo")]
    public int maxCombo = 2;
    public float comboResetTime = 1f;
    public float groundAttackDuration = 0.5f;
    public float airAttackDuration = 0.5f;
    private int comboIndex;
    private float lastAttackTime;
    private bool isAttacking;

    [Header("Ladder")]
    public float climbSpeed = 4f;
    private bool inLadderZone;
    private bool isClimbing;
    private int ladderContacts;
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

    // External toggle for CameraAutoMove
    public bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        if (!canMove) return;

        // 1) Read inputs & ground state
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isGrounded = Physics2D.OverlapCircle(
                              groundCheck.position,
                              groundCheckRadius,
                              groundLayer
                          );
        if (isGrounded) jumpCount = 0;

        // 2) Ladder logic
        if (inLadderZone && Mathf.Abs(verticalInput) > 0.1f && !isClimbing)
            StartClimbing();
        if (isClimbing && (!inLadderZone || isGrounded))
            StopClimbing();

        // 3) Actions
        HandleJump();
        HandleRoll();
        HandleAttack();

        // 4) Animator parameters
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0.1f); // <-- Added for isRunning condition
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetFloat("ClimbSpeed", Mathf.Abs(verticalInput));

        // 5) Flip sprite
        if (!isClimbing && horizontalInput != 0f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Sign(horizontalInput) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // re-check ground
        isGrounded = Physics2D.OverlapCircle(
                         groundCheck.position,
                         groundCheckRadius,
                         groundLayer
                     );
        if (isGrounded) jumpCount = 0;

        // 1) Climbing
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
            return;
        }

        // 2) Steering during attack/dash
        if (isAttacking)
        {
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            return;
        }

        // 3) Normal movement
        rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            (isGrounded || jumpCount < maxJumps))
        {
            if (isClimbing)
                StopClimbing();

            jumpCount++;
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    private void HandleRoll()
    {
        // Debug log for troubleshooting
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Debug.Log("LeftShift pressed. isGrounded: " + isGrounded + ", isClimbing: " + isClimbing + ", isAttacking: " + isAttacking + ", time since last roll: " + (Time.time - lastRollTime));

        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            isGrounded &&
            !isClimbing &&
            !isAttacking &&
            Time.time - lastRollTime >= rollCooldown)
        {
            lastRollTime = Time.time;
            isAttacking = true;
            animator.SetTrigger("Roll");
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {
        float end = Time.time + rollDuration;
        while (Time.time < end)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(transform.localScale.x) * rollForce,
                                       rb.linearVelocity.y);
            yield return null;
        }
        isAttacking = false;
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) &&
            !isClimbing &&
            !isAttacking)
        {
            if (Time.time - lastAttackTime > comboResetTime)
                comboIndex = 0;

            comboIndex = Mathf.Clamp(comboIndex + 1, 1, maxCombo);
            lastAttackTime = Time.time;
            isAttacking = true;

            bool grounded = isGrounded;
            string trigger = grounded
                ? $"Attack{comboIndex}"
                : "JumpAttack";
            animator.SetTrigger(trigger);

            // STOP any residual sliding immediately
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            float dur = grounded ? groundAttackDuration : airAttackDuration;
            StartCoroutine(EndAttackAfter(dur));
        }
    }

    private IEnumerator EndAttackAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    private void StartClimbing()
    {
        isClimbing = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        animator.Play("Climb", 0, 0f);
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = originalGravity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            ladderContacts++;
            inLadderZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            ladderContacts = Mathf.Max(0, ladderContacts - 1);
            if (ladderContacts == 0)
            {
                inLadderZone = false;
                if (isClimbing)
                    StopClimbing();
            }
        }
    }
}
