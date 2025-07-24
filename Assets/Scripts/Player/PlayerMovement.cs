using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private bool isClimbing = false;
    private float verticalInput;
    private float originalGravity;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float horizontalInput;
    public bool canMove = true;
    private bool isGrounded = false;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Animator animator;
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
        animator.SetBool("isClimbing", isClimbing);
        if (isClimbing)
        {
            // Tắt lực rơi
            rb.gravityScale = 0;

            // Di chuyển lên/xuống
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * moveSpeed);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
        else
        {
            // Bật lại trọng lực nếu không leo
            rb.gravityScale = originalGravity;
        }
        // Nhảy nếu đang chạm đất và nhấn Space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        // Lật hướng nhân vật
        if (horizontalInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(horizontalInput) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // Di chuyển ngang
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Kiểm tra đang đứng trên đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.linearVelocity = Vector2.zero; // Dừng rơi ngay khi vào thang
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}
