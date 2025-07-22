using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour
{
    private void Reset()
    {
        // Automatically set BoxCollider2D as Trigger
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        gameObject.tag = "Ladder";
    }

    private void OnDrawGizmos()
    {
        // Draw the ladder area in Scene view
        Gizmos.color = new Color(0f, 1f, 0.3f, 0.2f);
        if (TryGetComponent<BoxCollider2D>(out var col))
        {
            Gizmos.DrawCube(transform.position + (Vector3)col.offset, col.size);
        }
    }

    private void OnValidate()
    {
        // Ensure it always stays as trigger
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning("Ladder collider must be set to 'Is Trigger'. Automatically fixed.");
        }

        if (tag != "Ladder")
        {
            tag = "Ladder";
            Debug.LogWarning("Ladder object must have tag 'Ladder'. Automatically fixed.");
        }
    }
}
