using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public Sprite FullHeartSprite;
    public Sprite EmptyHeartSprite;
    public Image[] HeartImages;
    public PlayerHealth playerHealth;

    void Update()
    {
        if (playerHealth == null) return;
        CurrentHealth = playerHealth.CurrentHealth;
        MaxHealth = playerHealth.MaxHealth;
        for (int i = 0; i < HeartImages.Length; i++)
        {
            if (i < CurrentHealth)
                HeartImages[i].sprite = FullHeartSprite;
            else
                HeartImages[i].sprite = EmptyHeartSprite;
        }
    }
}
