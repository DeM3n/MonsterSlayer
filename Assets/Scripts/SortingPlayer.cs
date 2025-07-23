using UnityEngine;

public class SortingPlayer : MonoBehaviour
{
    public string sortingLayerName = "Default";
    public int sortingOrder = 0;
    public bool applyOnStart = true;

    void Start()
    {
        if (applyOnStart)
        {
            ApplySortingLayer();
        }
    }

    public void ApplySortingLayer()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sortingLayerName = sortingLayerName;
            sr.sortingOrder = sortingOrder;
        }
    }
}
