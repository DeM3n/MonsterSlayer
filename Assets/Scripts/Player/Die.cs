using UnityEngine;
using UnityEngine.SceneManagement;
public class FallDeath : MonoBehaviour
{
    public float fallThreshold = -8f;  // Độ cao giới hạn, ví dụ -10
    public GameUIManager  uiManager;
     public PlayerRespawn playerRespawn; 
      private bool hasFallen = false;  
    void Update()
    {
       if (!hasFallen && transform.position.y < fallThreshold)
        {
            hasFallen = true;
            Debug.Log("☠️ Player đã rơi khỏi map!");

            playerRespawn.Die();                    // Gọi die() của PlayerRespawn để xử lý
        }
    }

    public void ResetFallState()
    {
        hasFallen = false; // Cho phép kiểm tra lại rơi lần tiếp theo
    }
    
}
