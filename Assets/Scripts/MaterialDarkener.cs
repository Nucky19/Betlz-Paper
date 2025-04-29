using UnityEngine;

public class MaterialDarkener : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public Renderer targetRenderer;

    public Color startColor = Color.white;
    public Color endColor = Color.black;

    private Material materialInstance;

    void Start()
    {
        // Instancia el material para que este objeto tenga su propia copia
        materialInstance = new Material(targetRenderer.sharedMaterial);
        targetRenderer.material = materialInstance;
    }

    void Update()
    {
        float totalDistance = Vector3.Distance(pointA.position, pointB.position);
        float currentDistance = Vector3.Distance(player.position, pointB.position);
        float t = Mathf.Clamp01(1f - (currentDistance / totalDistance));

        Color newColor = Color.Lerp(startColor, endColor, t);
        materialInstance.color = newColor;
    }
}
