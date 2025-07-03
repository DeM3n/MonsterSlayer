using UnityEngine;
using UnityEngine.SceneManagement;
public class GameUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public PlayerRespawn playerRespawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false); // Ẩn bảng Game Over khi bắt đầu
    }

    // Update is called once per frame
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // Hiển thị bảng Game Over

    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Khôi phục thời gian game
         gameOverPanel.SetActive(false);
       playerRespawn.Respawn();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f; // Khôi phục thời gian game
        SceneManager.LoadScene("Menu");
            CameraAutoMove.hasPlayedIntro = false;
    }
}
