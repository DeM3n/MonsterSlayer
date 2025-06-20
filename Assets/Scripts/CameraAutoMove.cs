using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< Updated upstream
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
=======
    public float moveSpeed = 5f;              // Tốc độ di chuyển
    public float leftBound = -10f;            // Vị trí đầu map (biên trái)
    public float rightBound = 10f;            // Vị trí cuối map (biên phải)

    private bool isMoving = true;
    private PlayerMovement player;

    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerMovement>(); // Using the new method to find the PlayerMovement component
        if (player != null)
        {
            player.canMove = false; // Khóa di chuyển ban đầu
        }

        Vector3 startPos = transform.position;
        startPos.x = rightBound;
        transform.position = startPos;
>>>>>>> Stashed changes
    }

    void Update()
    {
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
<<<<<<< Updated upstream
                velocity.y = jumpForce;
=======
                Vector3 pos = transform.position;
                pos.x = leftBound;
                transform.position = pos;
                isMoving = false; // Dừng di chuyển khi tới đầu map
                if (player != null)
                {
                    player.canMove = true; // Allow player movement after camera stops
                }
>>>>>>> Stashed changes
            }
            rb.linearVelocity = velocity;
        }

        if (moveX != 0)
        {
            transform.localScale = new Vector3(moveX > 0 ? 1 : -1, 1, 1);
        }
    }
<<<<<<< Updated upstream

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
=======
>>>>>>> Stashed changes
}
