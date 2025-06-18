using UnityEngine;
using UnityEngine.UI;
public class HeartDisplay : MonoBehaviour
{
    public int MaxHealth ;
    public int CurrentHealth ;
    public Sprite FullHeartSprite;
    public Sprite EmptyHeartSprite;
    public Image[] HeartImages;
     public PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth=playerHealth.CurrentHealth;
        MaxHealth = playerHealth.MaxHealth;
        for (int i = 0; i < HeartImages.Length; i++)
        {
            if (i < CurrentHealth)
            {
                HeartImages[i].sprite = FullHeartSprite;
            }
            else
            {
                HeartImages[i].sprite = EmptyHeartSprite;
            }
        }
    }
}
