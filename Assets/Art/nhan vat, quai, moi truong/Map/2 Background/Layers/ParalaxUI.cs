using UnityEngine;
using UnityEngine.UI;

public class ParallaxUI : MonoBehaviour
{
    public float scrollSpeed = 2f;

    private RectTransform[] layers = new RectTransform[2];
    private float width;

    void Start()
    {
        // Lấy 2 Image con
        if (transform.childCount < 2)
        {
            Debug.LogError("ParallaxUI requires 2 child objects!");
            return;
        }

        layers[0] = transform.GetChild(0).GetComponent<RectTransform>();
        layers[1] = transform.GetChild(1).GetComponent<RectTransform>();

        width = layers[0].rect.width;
    }

    void Update()
    {
        foreach (RectTransform layer in layers)
        {
            layer.anchoredPosition += new Vector2(scrollSpeed * Time.deltaTime, 0);
        }

        if (layers[0].anchoredPosition.x - width > 0)
        {
            layers[0].anchoredPosition = new Vector2(
                layers[1].anchoredPosition.x - width,
                layers[0].anchoredPosition.y
            );

            SwapLayers();
        }
    }

    void SwapLayers()
    {
        RectTransform temp = layers[0];
        layers[0] = layers[1];
        layers[1] = temp;
    }
}