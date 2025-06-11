using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    public float moveSpeed = 5f;              // tốc độ di chuyển
    public float leftBound = -10f;            // vị trí đầu map (biên trái)
    public float rightBound = 10f;            // vị trí cuối map (biên phải)

    private bool isMoving = true;

    void Start()
    {
        // Đặt camera ở vị trí cuối map (rightBound) khi bắt đầu
        Vector3 startPos = transform.position;
        startPos.x = rightBound;
        transform.position = startPos;
    }

    void Update()
    {
        if (isMoving)
        {
            // Di chuyển camera về trái (hướng về đầu map)
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            // Giới hạn không vượt quá leftBound
            if (transform.position.x <= leftBound)
            {
                Vector3 pos = transform.position;
                pos.x = leftBound;
                transform.position = pos;
                isMoving = false; // Dừng di chuyển khi tới đầu map
            }
        }
    }
}