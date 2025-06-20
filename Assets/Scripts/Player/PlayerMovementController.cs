using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool canMove = true;

    [Header("Ground Check Settings")]
    public Transform groundCheckPosition;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private bool isGrounded = false;
    private bool isFacingRight = true;
    private float horizontalInput;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            horizontalInput = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }

            FlipSprite();
        }

        UpdateAnimationStates();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        CheckIfGrounded();
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

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer);
    }

    void UpdateAnimationStates()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        if (!isGrounded && rb.linearVelocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (!isGrounded && rb.linearVelocity.y < -0.1f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
        }
    }
}
