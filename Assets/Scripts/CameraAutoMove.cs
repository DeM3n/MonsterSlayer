using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    [Header("Camera Settings")]
    public float moveSpeed = 5f;               // tốc độ di chuyển camera
    public float leftBound = -10f;             // vị trí đầu map (giới hạn trái)
    public float rightBound = 10f;             // vị trí cuối map (giới hạn phải)

    private bool isMoving = true;              // camera có đang di chuyển không
    private PlayerMovement player;             // tham chiếu tới player (để khóa mở di chuyển)

    void Start()
    {
        // Bảo đảm thời gian game đang chạy
        if (Time.timeScale == 0f) Time.timeScale = 1f;

        // Tìm player đầu tiên có script PlayerMovement (đảm bảo player đã có script này)
        player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.canMove = false; // Khóa di chuyển ban đầu
            Debug.Log("Player movement locked.");
        }
        else
        {
            Debug.LogWarning("PlayerMovement script not found!");
        }

        // Đặt camera tại vị trí bắt đầu (rightBound)
        transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        // Di chuyển camera dần về bên trái
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Nếu vượt qua biên trái, dừng lại
        if (transform.position.x <= leftBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
            isMoving = false;

            // Cho phép player bắt đầu di chuyển
            if (player != null)
            {
                player.canMove = true;
                Debug.Log("Player movement unlocked.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ gizmo để hiển thị vị trí biên trái và phải trong editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(leftBound, -10f, 0f), new Vector3(leftBound, 10f, 0f));
        Gizmos.DrawLine(new Vector3(rightBound, -10f, 0f), new Vector3(rightBound, 10f, 0f));
    }
}
