using UnityEngine;

public class Winning : MonoBehaviour
{

  public GameObject winPanel; // UI hiện khi chiến thắng
    public int requiredRunes = 4; // Số rune cần để thắng

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.runeCount >= requiredRunes)
            {
                Debug.Log("🎉 Chiến thắng! Đã thu thập đủ rune.");
                Time.timeScale = 0f; // Dừng game (tuỳ chọn)
                if (winPanel != null)
                    winPanel.SetActive(true); // Hiện UI chiến thắng
            }
            else
            {
                Debug.Log("❌ Bạn chưa có đủ rune để thoát khỏi nơi này.");
               
            }
        }
    }
}
