﻿using UnityEngine;

public class BattleTurtle : MonoBehaviour
{
    [Header("Status")]
    public bool isDead = false;
    public int maxHealth = 5;

    [Header("Movement")]
    public bool facingLeft = true;
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;

    [Header("Combat & Detection")]
    public Transform player;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public Animator animator;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 2f;
    private float fireTimer = 0f;

    private bool inRange = false;

    void Update()
    {
        if (isDead) return;

        if (maxHealth <= 0)
        {
            Die();
            return;
        }

        float playerDist = Vector2.Distance(transform.position, player.position);
        inRange = playerDist <= attackRange;

        if (inRange)
        {
            HandleFacing();

            if (playerDist > retrieveDistance)
            {
                animator.SetBool("Attack2", false);
                Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack2", true);
                fireTimer += Time.deltaTime;
                if (fireTimer >= fireCooldown)
                {
                    fireTimer = 0f;
                    Shoot();
                }
            }
        }
        else
        {
            Patrol();
        }
    }

    void HandleFacing()
    {
        if (player.position.x > transform.position.x && facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (player.position.x < transform.position.x && !facingLeft)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }

    void Patrol()
    {
        animator.SetBool("Attack2", false);
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
        if (!hit)
        {
            Flip();
        }
    }

    void Flip()
    {
        if (facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        facingLeft = !facingLeft;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        maxHealth -= damage;
        if (maxHealth <= 0) Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("isDead");
        Debug.Log(this.transform.name + " Died.");
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
        if (checkPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (firePoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(firePoint.position, 0.2f);
        }
    }
}
