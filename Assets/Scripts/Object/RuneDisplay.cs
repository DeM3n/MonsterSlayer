using UnityEngine;
using TMPro;

public class RuneDisplay : MonoBehaviour
{
    public TextMeshProUGUI runeText;
    public PlayerInventory playerInventory;

    void Start()
    {
        if (runeText == null)
            runeText = GameObject.Find("RuneText").GetComponent<TextMeshProUGUI>();

        if (playerInventory == null)
            playerInventory = Object.FindFirstObjectByType<PlayerInventory>();
    }

    void Update()
    {
        runeText.text = "x " + playerInventory.runeCount;
    }
}

