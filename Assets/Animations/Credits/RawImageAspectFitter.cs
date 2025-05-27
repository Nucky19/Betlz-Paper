using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageAspectFitter : MonoBehaviour
{
    public RenderTexture renderTexture;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (renderTexture != null)
        {
            FitAspect(renderTexture.width, renderTexture.height);
        }
    }

    void FitAspect(float width, float height)
    {
        float aspect = width / height;

        float currentHeight = rectTransform.sizeDelta.y;
        float newWidth = currentHeight * aspect;

        rectTransform.sizeDelta = new Vector2(newWidth, currentHeight);
    }
}
