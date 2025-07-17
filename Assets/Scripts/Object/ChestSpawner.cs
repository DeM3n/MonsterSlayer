using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
  public GameObject KeyPrefabs;     
    public Transform[] spawnPoints;       

    void Start()
    {
        SpawnChest();
    }

    void SpawnChest()
    {
        if (spawnPoints.Length != 2)
        {
            Debug.LogError("2 vị trí để spawn thiếu.");
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            Instantiate(KeyPrefabs, spawnPoints[i].position, Quaternion.identity);
        }

        Debug.Log("thành công spawn key");
    }
}
