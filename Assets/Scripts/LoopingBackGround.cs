using UnityEngine;

public class LoopingBackGround : MonoBehaviour
{
    public Transform[] backgrounds; // Phải gán đủ 3 bg: trái, giữa, phải
    public Transform mainCamera;

    private float backgroundWidth;

    void Start()
    {
        backgrounds[0].position = new Vector3(-backgroundWidth, 0, 0);
backgrounds[1].position = new Vector3(0, 0, 0);
backgrounds[2].position = new Vector3(backgroundWidth, 0, 0);
        if (backgrounds == null || backgrounds.Length != 3)
        {
            Debug.LogError("Please assign exactly 3 backgrounds in the Inspector.");
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        AdjustBackgroundImmediately();
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float camX = mainCamera.position.x;
            float bgX = backgrounds[i].position.x;

            // Nếu background ở quá xa camera bên phải
            if (camX - bgX > backgroundWidth)
            {
                // Tìm background xa nhất bên trái để chuyển nó sang phải
                float rightMostX = GetRightMostX();
                backgrounds[i].position = new Vector3(rightMostX + backgroundWidth, backgrounds[i].position.y, backgrounds[i].position.z);
            }

            // Nếu background ở quá xa camera bên trái
            else if (bgX - camX > backgroundWidth)
            {
                // Tìm background xa nhất bên phải để chuyển nó sang trái
                float leftMostX = GetLeftMostX();
                backgrounds[i].position = new Vector3(leftMostX - backgroundWidth, backgrounds[i].position.y, backgrounds[i].position.z);
            }
            
        }
    }

    float GetRightMostX()
    {
        float max = backgrounds[0].position.x;
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x > max)
                max = bg.position.x;
        }
        return max;
    }

    float GetLeftMostX()
    {
        float min = backgrounds[0].position.x;
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.x < min)
                min = bg.position.x;
        }
        return min;
    }
    void AdjustBackgroundImmediately()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float camX = mainCamera.position.x;
            float bgX = backgrounds[i].position.x;

            if (camX - bgX > backgroundWidth)
            {
                float rightMostX = GetRightMostX();
                backgrounds[i].position = new Vector3(rightMostX + backgroundWidth, backgrounds[i].position.y, backgrounds[i].position.z);
            }
            else if (bgX - camX > backgroundWidth)
            {
                float leftMostX = GetLeftMostX();
                backgrounds[i].position = new Vector3(leftMostX - backgroundWidth, backgrounds[i].position.y, backgrounds[i].position.z);
            }
        }
    }
public void ResetBackgroundPosition()
{
    // Đặt lại 3 background thành: Trái - Giữa - Phải so với camera hiện tại
    Vector3 camPos = mainCamera.position;

    backgrounds[0].position = new Vector3(camPos.x - backgroundWidth, backgrounds[0].position.y, backgrounds[0].position.z);
    backgrounds[1].position = new Vector3(camPos.x, backgrounds[1].position.y, backgrounds[1].position.z);
    backgrounds[2].position = new Vector3(camPos.x + backgroundWidth, backgrounds[2].position.y, backgrounds[2].position.z);
}
}
