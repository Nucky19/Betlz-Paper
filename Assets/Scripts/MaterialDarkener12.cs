using UnityEngine;

public class MaterialDarkener2 : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public Renderer targetRenderer;

    public Color startColor = Color.white;
    public Color endColor = Color.black;

    void Update()
    {
        float totalDistance = Vector3.Distance(pointA.position, pointB.position);
        float currentDistance = Vector3.Distance(player.position, pointB.position);

        // Asegura que el progreso esté entre 0 y 1
        float t = Mathf.Clamp01(1f - (currentDistance / totalDistance));

        // Interpola el color del material
        Color newColor = Color.Lerp(startColor, endColor, t);
        targetRenderer.material.color = newColor;
    }
}
