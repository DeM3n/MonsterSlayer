using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadSceneAsync("Level1");
    }
       public void QuitGame()
    {
        Debug.Log("Quit Game requested");

        // Nếu đang chạy trong Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Nếu đang chạy bản build
        Application.Quit();
#endif
    }
}
