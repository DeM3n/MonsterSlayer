using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 14f;
    public bool canMove = true;

    [Header("Climbing Settings")]
    public float climbSpeed = 3f;
    private bool isClimbing = false;
    private bool isNearLadder = false;

    [Header("Roll / Dash Settings")]
    public float rollSpeed = 10f;
    public float rollDuration = 0.4f;
    public float rollCooldown = 1f;
    private bool isRolling = false;
    private float rollTimer = 0f;
    private float rollCooldownTimer = 0f;
    private bool isInvincible = false;

    [Header("Attack Settings")]
    public float attackCooldown = 0.4f;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded = false;

    private bool isFacingRight = true;
    private float horizontalInput;
    private float verticalInput;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // === Ladder Climbing ===
        if (isNearLadder && Mathf.Abs(verticalInput) > 0.1f)
            isClimbing = true;

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3f;
        }

        // === Jumping ===
        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing && !isRolling)
            Jump();

        if (isClimbing && (Mathf.Abs(horizontalInput) > 0.1f || Input.GetButtonDown("Jump")))
            StopClimbing();

        // === Rolling ===
        if (!isRolling && rollCooldownTimer <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(PerformRoll());

        if (rollCooldownTimer > 0f)
            rollCooldownTimer -= Time.deltaTime;

        // === Attacking ===
        if (!isAttacking && Input.GetMouseButtonDown(0))
            StartCoroutine(PerformAttack());

        // === Flip & Anim ===
        FlipSprite();
        UpdateAnimationStates();
    }

    void FixedUpdate()
    {
        CheckIfGrounded();

        if (canMove && !isClimbing && !isRolling)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    void UpdateAnimationStates()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("isClimbing", isClimbing && Mathf.Abs(verticalInput) > 0.1f);
        animator.SetBool("isRolling", isRolling);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isGrounded", isGrounded);

        if (!isGrounded && rb.linearVelocity.y > 0.1f && !isClimbing && !isRolling)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded && rb.linearVelocity.y < -0.1f && !isClimbing && !isRolling)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else if (isGrounded && !isClimbing && !isRolling)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    void CheckIfGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        if (wasGrounded != isGrounded)
            Debug.Log("Grounded: " + isGrounded);
    }

    IEnumerator PerformRoll()
    {
        isRolling = true;
        isInvincible = true;
        rollTimer = rollDuration;
        rollCooldownTimer = rollCooldown;

        animator.SetTrigger("Roll");

        float dir = isFacingRight ? 1f : -1f;

        while (rollTimer > 0f)
        {
            rb.linearVelocity = new Vector2(dir * rollSpeed, 0f);
            rollTimer -= Time.deltaTime;
            yield return null;
        }

        isRolling = false;
        isInvincible = false;
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // === External helpers ===
    public void EnterLadderZone() => isNearLadder = true;
    public void ExitLadderZone() { isNearLadder = false; StopClimbing(); }
    public void StopClimbing() { isClimbing = false; rb.gravityScale = 3f; }
    public bool IsInvincible() => isInvincible;
    public void SetGrounded(bool grounded) => isGrounded = grounded;
}
