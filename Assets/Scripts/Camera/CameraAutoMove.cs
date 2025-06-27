using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float leftBound = 0f;
    public float rightBound = 250f;

    private bool isMoving = true;
    private PlayerMovement player;
    private CameraFollow cameraFollow;
    public GameObject heartDisplay;
    public GameObject keyDisplay;
    public GameObject runeDisplay;
    public static bool hasPlayedIntro = false;
    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerMovement>();
        cameraFollow = GetComponent<CameraFollow>();
         if (hasPlayedIntro)
        {
            // ✅ Nếu đã chạy Intro rồi → Bỏ qua luôn, bật follow + UI ngay
            if (player != null) player.canMove = true;
            if (cameraFollow != null) cameraFollow.followEnabled = true;

            if (heartDisplay != null) heartDisplay.SetActive(true);
            if (keyDisplay != null) keyDisplay.SetActive(true);
            if (runeDisplay != null) runeDisplay.SetActive(true);
              LoopingBackGround bgLooper = Object.FindFirstObjectByType<LoopingBackGround>();
        if (bgLooper != null)
        {
            bgLooper.ResetBackgroundPosition();
        }

            return;  // ✅ Thoát luôn khỏi Start, không chạy auto-move
        }
        if (player != null)
            player.canMove = false;

        if (cameraFollow != null)
            cameraFollow.followEnabled = false;

        Vector3 startPos = transform.position;
        startPos.x = rightBound;
        transform.position = startPos;

        if (heartDisplay != null) heartDisplay.SetActive(false);
        if (keyDisplay != null) keyDisplay.SetActive(false);
        if (runeDisplay != null) runeDisplay.SetActive(false);
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x <= leftBound)
            {
                transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
                isMoving = false;

                if (player != null)
                    player.canMove = true;

                if (cameraFollow != null)
                {
                    // Fix vị trí trước khi follow
                    Vector3 targetPos = player.transform.position + cameraFollow.offset;
                    targetPos.y = transform.position.y;
                    targetPos.z = transform.position.z;
                    targetPos.x = Mathf.Clamp(targetPos.x, cameraFollow.leftBound, cameraFollow.rightBound);
                    transform.position = targetPos;

                    cameraFollow.followEnabled = true;

                }
                  if (heartDisplay != null) heartDisplay.SetActive(true);
if (keyDisplay != null) keyDisplay.SetActive(true);
                if (runeDisplay != null) runeDisplay.SetActive(true);
    hasPlayedIntro = true;
            }
        }
      
    }
    
}
