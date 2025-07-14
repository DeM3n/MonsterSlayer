using UnityEngine;
using System.Collections;

public class FallingTile : MonoBehaviour
{
    public float fallDelay = 0.3f;
    public float destroyAfter = 2f;

    private Rigidbody2D rb;
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

        // Hiệu ứng rung
        for (int i = 0; i < 5; i++)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        transform.position = originalPos;

        // Sau 1 khoảng delay, bắt đầu rơi
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }
}
