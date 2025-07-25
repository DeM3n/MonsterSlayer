using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameUIManager gameUIManager;
    private PlayerMovement movementScript;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void Die()
    {
        Debug.Log("💀 Player died");
        if (gameUIManager != null)
            gameUIManager.ShowGameOver();
        if (movementScript != null)
            movementScript.enabled = false;
    }

    public void Respawn()
    {
        Debug.Log("🔁 Respawn called");

        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager.Instance is NULL! Cannot get checkpoint.");
            return;
        }

        Vector3 checkpoint = GameManager.Instance.GetCurrentCheckpoint();
        transform.position = checkpoint;
        gameObject.SetActive(true);

        // Reset FallDeath state nếu có
        FallDeath fallDeath = GetComponent<FallDeath>();
        if (fallDeath != null)
        {
            fallDeath.ResetFallState();
        }

        // Reset máu + enable movement
        if (playerHealth != null)
            playerHealth.ResetHealth();
        if (movementScript != null)
            movementScript.enabled = true;

        Debug.Log("✅ Player respawned at " + checkpoint);
    }
}
