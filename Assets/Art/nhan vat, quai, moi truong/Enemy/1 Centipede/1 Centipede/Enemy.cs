using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDead = false;
    public int maxHealth = 5;
    public bool facingLeft = true;
    public float moveSpeed = 2f;

    public Transform checkPoint;           // để kiểm tra nền
    public Transform wallCheck;            // để kiểm tra tường
    public float distance = 1f;            // khoảng cách kiểm tra nền
    public float wallCheckDistance = 0.2f; // khoảng cách kiểm tra tường
    public LayerMask layerMask;

    public bool inRange = false;
    public Transform player;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;

    public Animator animator;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        if (maxHealth <= 0) { Die(); return; }

        float dist = Vector2.Distance(transform.position, player.position);
        inRange = dist <= attackRange;

        if (inRange)
        {
            FacePlayer();

            if (dist > retrieveDistance)
            {
                animator.SetBool("Attack 1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack 1", true);
            }
        }
        else
        {
            animator.SetBool("Attack 1", false);
            Patrol();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        RaycastHit2D groundHit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, facingLeft ? Vector2.left : Vector2.right, wallCheckDistance, layerMask);

        if ((!groundHit || wallHit) && facingLeft)
        {
            FlipRight();
        }
        else if ((!groundHit || wallHit) && !facingLeft)
        {
            FlipLeft();
        }

        Debug.DrawRay(checkPoint.position, Vector2.down * distance, Color.yellow); // nền
        Debug.DrawRay(wallCheck.position, (facingLeft ? Vector2.left : Vector2.right) * wallCheckDistance, Color.cyan); // tường
    }

    void FacePlayer()
    {
        if (player.position.x > transform.position.x && facingLeft)
            FlipRight();
        else if (player.position.x < transform.position.x && !facingLeft)
            FlipLeft();
    }

    void FlipRight()
    {
        transform.eulerAngles = new Vector3(0, -180, 0);
        facingLeft = false;
    }

    void FlipLeft()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        facingLeft = true;
    }

    // GỌI bằng Animation Event trong anim "Attack"
    public void Attack()
    {
        if (isDead) return;

        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            PlayerHealth playerHealth = collInfo.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || maxHealth <= 0) return;

        maxHealth -= damage;
        animator.SetTrigger("Hurt");

        if (maxHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        animator.SetBool("Attack 1", false);
        animator.SetTrigger("isDead");
        Debug.Log(this.transform.name + " Died.");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        this.enabled = false;
    }

#if UNITY_EDITOR
    [ContextMenu("Test Die")]
    public void TestDie()
    {
        TakeDamage(maxHealth);
    }
#endif

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null || wallCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(wallCheck.position, Vector2.left * wallCheckDistance);
        Gizmos.DrawRay(wallCheck.position, Vector2.right * wallCheckDistance);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
