using UnityEngine;
using UnityEngine.SceneManagement;
public class FallDeath : MonoBehaviour
{
    public float fallThreshold = -10f;  // Độ cao giới hạn, ví dụ -10
    public GameUIManager  uiManager;
    private bool isDead = false;
    void Update()
    {
        if ( !isDead &&  transform.position.y < fallThreshold)
        {
            Die();
        }
    }

     void Die()
    {
        isDead = true;
        Debug.Log("Player died by falling!");
        uiManager.ShowGameOver();
        Time.timeScale = 0f;
    }
    
}
