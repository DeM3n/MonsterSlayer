using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage.");

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died.");

        if (animator != null)
            animator.SetTrigger("Die");

        // Disable enemy behavior
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Optional: destroy the enemy after animation
        Destroy(gameObject, 1f);
    }
}
