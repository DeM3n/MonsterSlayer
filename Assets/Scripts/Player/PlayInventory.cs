using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int keyCount = 0;
    public int runeCount = 0; // Biến này có thể dùng để đếm số lượng vật phẩm khác nếu cần
    // Hàm gọi khi nhặt chìa
    public void AddKey(int amount = 1)
    {
        keyCount += amount;
        Debug.Log("Nhặt được chìa khóa! Tổng số: " + keyCount);
    }

    // Hàm gọi khi dùng chìa
    public bool UseKey()
    {

        if (keyCount > 0)
        {
            keyCount--;

            return true;
        }
        else
        {

            return false;
        }
    }
    public void AddRune(int amount = 1)
    {
        runeCount += amount;
        Debug.Log("Nhặt được vật phẩm! Tổng số: " + runeCount);
    }
}