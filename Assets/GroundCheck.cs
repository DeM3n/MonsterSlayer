using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerMovementController player;

    void Start()
    {
        player = GetComponentInParent<PlayerMovementController>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            player.SetGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            player.SetGrounded(false);
        }
    }
}
