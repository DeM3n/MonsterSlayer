using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public PlayerRespawn playerRespawn;
    public PlayerHealth playerHealth;

    void Start()
    {
        gameOverPanel.SetActive(false);
        if (playerHealth == null) Debug.LogError("PlayerHealth is not assigned!");
        if (playerRespawn == null) Debug.LogError("PlayerRespawn is not assigned!");
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        playerRespawn.Respawn();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        CameraAutoMove.hasPlayedIntro = false;
    }

}
