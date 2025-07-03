using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetCurrentCheckpoint(transform.position);
            Debug.Log("Checkpoint updated: " + transform.position);
        }
    }
}