using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // Vẽ gizmo trong Scene view để dễ chỉnh vùng ladder
        Gizmos.color = new Color(0f, 1f, 0.3f, 0.2f);
        if (TryGetComponent<BoxCollider2D>(out var col))
        {
            Gizmos.DrawCube(transform.position + (Vector3)col.offset, col.size);
        }
    }
}
