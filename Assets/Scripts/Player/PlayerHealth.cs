using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    public int MaxHealth = 5;
    public int CurrentHealth;
    private bool isDead = false;
    public PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        CurrentHealth -= damage;
        animator.SetTrigger("Hurt");
        Debug.Log("Player took damage. Current Health: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("Die");
        Debug.Log("Player has died.");
        if (playerMovement != null)
            playerMovement.enabled = false;
        GameUIManager ui = FindObjectOfType<GameUIManager>();
        if (ui != null) ui.ShowGameOver();
    }

    public void SetHealthtoZero()
    {
        if (!isDead)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        isDead = false;
        if (playerMovement != null)
            playerMovement.enabled = true;
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        Debug.Log("Player healed. Current Health: " + CurrentHealth);
    }
}
