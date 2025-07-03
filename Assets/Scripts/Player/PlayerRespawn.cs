using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
   public GameUIManager gameUIManager;
private PlayerMovement movementScript;
    
    private void Awake()
    {
        movementScript = GetComponent<PlayerMovement>();
    }
    public void Die()
    {
        Debug.Log("💀 Player died");
        gameUIManager.ShowGameOver();
        if (movementScript != null)
            movementScript.enabled = false;
    }

    public void Respawn()
    {
        Vector3 checkpoint = GameManager.Instance.GetCurrentCheckpoint();
        transform.position = checkpoint;
        gameObject.SetActive(true); // Đảm bảo player được bật lại
         FallDeath fallDeath = GetComponent<FallDeath>();
    if (fallDeath != null)
    {
        fallDeath.ResetFallState();  // Gọi hàm reset biến cờ
    } 
    if (movementScript != null)
            movementScript.enabled = true;

        Debug.Log("✅ Player respawned at " + checkpoint);
    }
}
