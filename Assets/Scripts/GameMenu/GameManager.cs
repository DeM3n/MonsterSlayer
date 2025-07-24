using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    public static GameManager Instance { get; private set; }

    [Header("Start Point")]
    public Transform initialSpawnPoint;

    private Vector3 currentCheckpoint;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentCheckpoint = initialSpawnPoint != null ? initialSpawnPoint.position : new Vector3(0f, 5f, 0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
    }

    public Vector3 GetCurrentCheckpoint()
    {
        return currentCheckpoint;
    }
}
