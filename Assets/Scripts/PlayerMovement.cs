using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float climbSpeed = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded = false;
    private bool isNearStair = false;
    private bool isClimbing = false;

    public bool canMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!canMove) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (isNearStair && Input.GetKeyDown(KeyCode.E))
        {
            isClimbing = true;
            rb.linearVelocity = Vector2.zero;
        }

        if (isClimbing && Input.GetKeyDown(KeyCode.Space))
        {
            isClimbing = false;
            rb.gravityScale = 3;
        }

        if (isClimbing)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3;
            Vector2 velocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                velocity.y = jumpForce;
            }
            rb.linearVelocity = velocity;
        }

        if (moveX != 0)
        {
            transform.localScale = new Vector3(moveX > 0 ? 1 : -1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
        {
            isNearStair = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stair"))
        {
            isNearStair = false;
            isClimbing = false;
            rb.gravityScale = 3;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}