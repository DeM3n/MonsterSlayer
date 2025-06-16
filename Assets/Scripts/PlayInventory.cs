using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int keyCount = 0;

    // Hàm gọi khi nhặt chìa
    public void AddKey(int amount = 1)
    {
        keyCount += amount;
        Debug.Log("Nhặt được chìa khóa! Tổng số: " + keyCount);
    }

    // Hàm gọi khi dùng chìa
    public bool UseKey()
    {
          Debug.Log("Dùng chìa? keyCount = " + keyCount);
        if (keyCount > 0)
        {
            keyCount--;
            Debug.Log("Đã dùng 1 chìa khóa! Còn lại: " + keyCount);
            return true;
        }
        else
        {
            Debug.Log("Không có chìa khóa để dùng!");
            return false;
        }
    }
}