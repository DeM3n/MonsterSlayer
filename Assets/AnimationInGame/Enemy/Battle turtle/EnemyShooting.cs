using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject[] bullets; 
    public Transform[] bulletPositions;

    private float timer;
    private GameObject player;
    private int currentBulletIndex = 0;
    private int currentPosIndex = 0;

    [Header("State")]
    public bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isDead || player == null) return;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 10)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0f;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (isDead) return;

        if (bullets.Length == 0 || bulletPositions.Length == 0)
        {
            Debug.LogWarning("Bullet or Bullet Positions not assigned.");
            return;
        }

        GameObject bulletToShoot = bullets[currentBulletIndex];
        Transform shootPos = bulletPositions[currentPosIndex];
        Instantiate(bulletToShoot, shootPos.position, Quaternion.identity);

        currentBulletIndex = (currentBulletIndex + 1) % bullets.Length;
        currentPosIndex = (currentPosIndex + 1) % bulletPositions.Length;
    }
}
