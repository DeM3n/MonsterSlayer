using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 4f;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float horizontalInput;
    public bool canMove = true;
    private bool isGrounded = false;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Nhảy nếu đang chạm đất và nhấn Space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
}