using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float radius = 0.5f;
    public LayerMask enemyLayer;
    public int damage = 1;

    public void PerformAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (var hit in hits)
        {
            hit.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
