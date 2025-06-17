using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isHanging;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    private bool isDashing;

    [Header("Roll Settings")]
    public float rollSpeed = 12f;
    public float rollDuration = 0.3f;
    private bool isRolling;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Friction Settings")]
    public float friction = 0.9f;

    [Header("Climb Settings")]
    private bool isOnStairs;
    public float climbSpeed = 4f;

    [Header("Ledge Grab Settings")]
    public LayerMask ledgeLayer;

    private Animator animator;
    private Vector2 facingDirection => new Vector2(transform.localScale.x, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 2f;
        rb.freezeRotation = true;
        currentHealth = maxHealth;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Flip player
        if (horizontal != 0 && !isDashing && !isRolling)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);
        }

        // Movement
        if (!isDashing && !isRolling && !isHanging && !isOnStairs)
        {
            float targetVelocityX = horizontal * moveSpeed;
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, targetVelocityX, friction), rb.linearVelocity.y);
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        // Ground Check
        if (groundCheck != null)
        {
            // Use OverlapCircle if GroundCheck is assigned
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Fallback Raycast from player center
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        }
        animator.SetBool("Jumping", !isGrounded);

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash(horizontal));
            animator.SetTrigger("Dash");
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.C) && !isRolling)
        {
            StartCoroutine(Roll(horizontal));
            animator.SetTrigger("Roll");
        }

        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            animator.SetTrigger("Attack");
        }

        // Climb stairs
        if (isOnStairs)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
            animator.SetBool("Climbing", vertical != 0);
        }
        else if (!isHanging)
        {
            rb.gravityScale = 2f;
            animator.SetBool("Climbing", false);
        }

        // Ledge Grab
        if (!isGrounded && !isHanging && !isOnStairs)
        {
            RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, facingDirection, 0.6f, ledgeLayer);
            RaycastHit2D belowCheck = Physics2D.Raycast(transform.position + Vector3.down * 0.5f, Vector2.down, 0.1f, ledgeLayer);

            if (wallCheck.collider != null && belowCheck.collider == null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0;
                isHanging = true;
                animator.SetBool("Hanging", true);
            }
        }

        // Climb up from ledge
        if (isHanging && Input.GetButtonDown("Jump"))
        {
            rb.gravityScale = 2;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isHanging = false;
            animator.SetBool("Hanging", false);
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player takes damage: " + damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        // Add death animation or respawn logic here
    }

    IEnumerator Dash(float direction)
    {
        isDashing = true;
        rb.linearVelocity = new Vector2(direction * dashSpeed, 0);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    IEnumerator Roll(float direction)
    {
        isRolling = true;
        rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);
        yield return new WaitForSeconds(rollDuration);
        isRolling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
            isOnStairs = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
            isOnStairs = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
