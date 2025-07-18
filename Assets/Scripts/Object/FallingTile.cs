using UnityEngine;
using System.Collections;

public class FallingTile : MonoBehaviour
{
    public float fallDelay = 0.3f;
    public float destroyAfter = 2f;

    private Rigidbody2D rb;
    public GameObject smokeEffectPrefab;             // Prefab hiệu ứng khói
    public Transform[] smokeSpawnPoints;   
    private bool hasTriggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(FallRoutine());
        }
    }

    IEnumerator FallRoutine()
    {
        Vector3 originalPos = transform.position;

        if (smokeEffectPrefab != null && smokeSpawnPoints != null && smokeSpawnPoints.Length >= 2)
        {
           var smoke1 = Instantiate(smokeEffectPrefab, smokeSpawnPoints[0].position, Quaternion.identity);
var smoke2 = Instantiate(smokeEffectPrefab, smokeSpawnPoints[1].position, Quaternion.identity);
Destroy(smoke1, 2f);
Destroy(smoke2, 2f);
        }

        // Hiệu ứng rung
        for (int i = 0; i < 5; i++)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.04f;
            yield return new WaitForSeconds(0.04f);
        }

        transform.position = originalPos;

        // Sau 1 khoảng delay, bắt đầu rơi
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
}
