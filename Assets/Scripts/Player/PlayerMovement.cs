using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public bool canMove = true;

    private Rigidbody2D rb;
    private float moveInput = 0f;
    private bool isJumping = false;
    private float jumpDirection = 0f;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;

        if (!isJumping)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }

        if (!isJumping && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpDirection = moveInput;
            rb.linearVelocity = new Vector2(jumpDirection * moveSpeed, jumpForce);
        }

        if (!isJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // Flip sprite
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(moveInput > 0 ? 1 : -1, 1, 1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
