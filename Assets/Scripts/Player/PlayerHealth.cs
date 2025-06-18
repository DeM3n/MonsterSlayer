        using UnityEngine;

        public class PlayerHealth : MonoBehaviour

        {
            private Animator animator;
            public int MaxHealth = 5;
            public int CurrentHealth ;
            private bool isDead = false; // Trạng thái của người chơi
            public PlayerMovement playerMovement;
            void Start()
            {
                CurrentHealth = MaxHealth; // Initialize current health to max health
            }

            // Update is called once per frame
  public void TakeDamage(int damage)
    {
        if (isDead) return; // Nếu đã chết thì không nhận thêm dame

        CurrentHealth -= damage;
        Debug.Log("Player took damage. Current Health: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die"); // Cần trigger "Die" trong Animator
        Debug.Log("Player has died.");
         if (playerMovement != null)
            playerMovement.enabled = false;
    }
   }
