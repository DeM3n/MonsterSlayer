using UnityEngine;
using System.Collections.Generic;

public class RuneSpawner : MonoBehaviour
{
    public GameObject runePrefab;
    public Transform[] spawnPoints;  // Kéo 8 vị trí vào đây
    public int runeCount = 4;        // Số rune cần spawn

    private void Start()
    {
        SpawnRunesAtFixedPositions();
    }

    void SpawnRunesAtFixedPositions()
    {
        if (spawnPoints.Length < runeCount)
        {
            Debug.LogError("Không đủ vị trí spawn! Bạn cần ít nhất " + runeCount + " điểm spawn.");
            return;
        }

        // Tạo danh sách chỉ số ngẫu nhiên, không trùng lặp
        List<int> chosenIndexes = new List<int>();
        while (chosenIndexes.Count < runeCount)
        {
            int rand = Random.Range(0, spawnPoints.Length);
            if (!chosenIndexes.Contains(rand))
                chosenIndexes.Add(rand);
        }

        // Spawn rune tại các vị trí được chọn
        foreach (int index in chosenIndexes)
        {
            Transform point = spawnPoints[index];
            Instantiate(runePrefab, point.position, Quaternion.identity);
        }

        Debug.Log($"✅ Đã spawn {runeCount} rune tại các vị trí cố định.");
    }
}
