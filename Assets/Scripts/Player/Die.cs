using UnityEngine;

public class FallDeath : MonoBehaviour
{
    public float fallThreshold = -8f;
    public GameUIManager uiManager;
    public PlayerRespawn playerRespawn;
    private bool hasFallen = false;
    public PlayerHealth playerHealth;

    void Update()
    {
        if (!hasFallen && transform.position.y < fallThreshold)
        {
            hasFallen = true;
            Debug.Log("☠️ Player đã rơi khỏi map!");
            playerHealth.SetHealthtoZero();
            playerRespawn.Die();
        }
    }

    public void ResetFallState()
    {
        hasFallen = false;
    }
}
