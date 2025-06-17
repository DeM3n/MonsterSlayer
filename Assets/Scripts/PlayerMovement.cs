using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public bool canMove = false;

    private float moveInput;
    private float lockedDirection = 0;
    private bool isJumping = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        // Only update direction when not jumping
        if (!isJumping)
        {
            lockedDirection = moveInput;

            // Flip character when on ground
            if (moveInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput), 1f, 1f);
            }
        }

        // Apply movement (locked in air)
        Vector2 velocity = rb.linearVelocity;
        velocity.x = lockedDirection * moveSpeed;
        rb.linearVelocity = velocity;

        // Jump only once
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
        }

        // Reset jump when vertical speed is near 0
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            isJumping = false;
        }
    }
}
