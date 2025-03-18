using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Configuración de las plataformas")]
    [SerializeField] private List<Transform> platforms; // Lista de plataformas
    [SerializeField] private List<bool> initialDirections; // Direcciones iniciales (true = adelante, false = atrás)
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float separationSpeed = 5f; // 🔹 Velocidad reducida al colisionar

    private Dictionary<Transform, bool> movingForward = new Dictionary<Transform, bool>();
    private Dictionary<Transform, float> platformSpeeds = new Dictionary<Transform, float>(); // 🔹 Velocidad de cada plataforma

    private void Start()
    {
        if (platforms.Count != initialDirections.Count)
        {
            // Debug.LogError("Las listas de plataformas y direcciones iniciales deben tener el mismo tamaño.");
            return;
        }

        // Asignamos la dirección y velocidad inicial a cada plataforma
        for (int i = 0; i < platforms.Count; i++)
        {
            movingForward[platforms[i]] = initialDirections[i];
            platformSpeeds[platforms[i]] = moveSpeed;

            // Configurar el detector de colisiones
            PlatformCollisionDetector detector = platforms[i].GetComponentInChildren<PlatformCollisionDetector>();
            if (detector != null){
                detector.SetPlatformController(this, platforms[i]);
            }
        }


        if (PlatformEventManager.Instance != null){
            PlatformEventManager.Instance.OnPlatformReturn += ChangeDirection;
        }
    }

    private void Update(){
        MovePlatforms();
    }

    private void MovePlatforms(){
        foreach (Transform platform in platforms){
            float direction = movingForward[platform] ? 1 : -1;
            platform.position += Vector3.forward * direction * platformSpeeds[platform] * Time.fixedDeltaTime;
        }
    }

    private void ChangeDirection()
    {
        foreach (Transform platform in platforms)
        {
            movingForward[platform] = !movingForward[platform]; // Invertimos dirección
            platformSpeeds[platform] = moveSpeed; // 🔹 Restauramos la velocidad normal
        }

        // Debug.Log("Todas las plataformas cambiaron de dirección y restauraron velocidad.");
    }

    public void HandlePlatformCollision(Transform platform)
    {
        if (platforms.Contains(platform))
        {
            movingForward[platform] = !movingForward[platform];
            platformSpeeds[platform] = separationSpeed; // 🔹 Reducimos velocidad temporalmente
            // Debug.Log(platform.name + " chocó con otra plataforma. Reduciendo velocidad y cambiando dirección.");
        }
    }

    private void OnDestroy()
    {
        if (PlatformEventManager.Instance != null)
        {
            PlatformEventManager.Instance.OnPlatformReturn -= ChangeDirection;
        }
    }
}
