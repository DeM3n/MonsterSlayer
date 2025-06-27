using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(3f, 0f, -10f);
    public bool followEnabled = false;

    public float leftBound = -10f;
    public float rightBound = 100f;

    void LateUpdate()
    {
        if (!followEnabled || target == null) return;

        // Tính vị trí mong muốn (có offset)
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);

        // Giới hạn theo biên
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftBound, rightBound);

        // Di chuyển mượt
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
