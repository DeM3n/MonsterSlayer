using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KeyDisplay : MonoBehaviour
{
    public TextMeshProUGUI  keyText;
    public PlayerInventory playerInventory;

    void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = Object.FindFirstObjectByType<PlayerInventory>();
        }
    }

    void Update()
    {
        keyText.text = "x " + playerInventory.keyCount;
    }
}
