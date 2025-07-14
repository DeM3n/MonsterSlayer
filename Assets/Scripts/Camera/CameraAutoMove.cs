using UnityEngine;
using TMPro;                
using System.Collections;
[System.Serializable]
public class SubtitleEntry
{
    public string text;
    public float delayBefore;         // Thời gian chờ trước khi hiển thị
    public AudioClip voiceClip;       // Voice kèm theo (nếu có)
}
public class CameraAutoMove : MonoBehaviour
{
    [Header("Lore Voice & Subtitle")]
    public SubtitleEntry[] subtitles;
    public TMPro.TextMeshProUGUI subtitleText;
    public AudioSource audioSource;
    private bool subtitleStarted = false;
    public float moveSpeed = 15f;
    public float leftBound = 0f;
    public float rightBound = 250f;

    private bool isMoving = true;
    private PlayerMovement player;
    private CameraFollow cameraFollow;
    public GameObject heartDisplay;
    public GameObject keyDisplay;
    public GameObject runeDisplay;
    public GameObject subtitleCanvas;
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
        if (subtitleCanvas != null) subtitleCanvas.SetActive(true);
         StartCoroutine(PlayIntroSubtitles());
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
    IEnumerator PlayIntroSubtitles()
{
    if (subtitleStarted) yield break;
    subtitleStarted = true;

    foreach (var entry in subtitles)
    {
        yield return new WaitForSeconds(entry.delayBefore);

        if (subtitleText != null)
            subtitleText.text = entry.text;

        if (audioSource != null && entry.voiceClip != null)
        {
            audioSource.clip = entry.voiceClip;
            audioSource.Play();
        }

        float duration = entry.voiceClip != null ? entry.voiceClip.length : 3f;
        yield return new WaitForSeconds(duration);

        if (subtitleText != null)
            subtitleText.text = "";
    }
}
    
}
