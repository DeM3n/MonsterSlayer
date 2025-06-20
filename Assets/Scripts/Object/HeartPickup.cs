using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int amount = 1;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(amount);
                Destroy(gameObject); // Ẩn rune khỏi map
            }
        }
    }
}
