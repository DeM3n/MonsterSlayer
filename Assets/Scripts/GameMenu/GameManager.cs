using UnityEngine;

public class GameManager : MonoBehaviour
{
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
            currentCheckpoint = initialSpawnPoint != null ? initialSpawnPoint.position : Vector3.zero;
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
