using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float verticalInput;
    private bool isNearLadder;

    [SerializeField] private Rigidbody2D rb;          // Player's Rigidbody2D
    [SerializeField] private PlayerMovement playerMovement;  // Reference to PlayerMovement for controlling movement

    private float climbSpeed = 5f;  // Speed at which the player climbs the ladder

    void Update()
    {
        // Only allow climbing if the player is near the ladder and presses the vertical input (W or Up Arrow)
        if (isNearLadder && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Get vertical input for climbing (W or Up arrow for climbing up, S or Down arrow for climbing down)
            verticalInput = Input.GetAxisRaw("Vertical");

            // Set the player's vertical velocity based on input
            rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed); // Stop horizontal velocity during climbing

            // Disable horizontal movement when climbing
            playerMovement.canMove = false; // Disable horizontal movement while climbing
        }
        else
        {
            if (isNearLadder)
            {
                playerMovement.canMove = true;  // Enable horizontal movement again when not climbing
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNearLadder = true;  // Player is near the ladder
            rb.gravityScale = 0f;  // Turn off gravity while near the ladder
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNearLadder = false;  // Player is no longer near the ladder
            rb.gravityScale = 4f;  // Restore gravity after leaving the ladder
            playerMovement.canMove = true;  // Enable horizontal movement again
        }
    }
}
