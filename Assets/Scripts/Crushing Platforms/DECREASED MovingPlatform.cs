using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float separationSpeed = 3f;
    [SerializeField] private bool movingForward = true; // Dirección actual

    private bool isSeparating = false; // Indica si la plataforma está separándose después del choque
    private Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;

        // Asegurar que solo nos suscribimos una vez
        if (PlatformEventManager.Instance != null)
        {
            PlatformEventManager.Instance.OnPlatformReturn -= ChangeDirection; // 🔹 Primero nos desuscribimos por si acaso
            PlatformEventManager.Instance.OnPlatformReturn += ChangeDirection; // 🔹 Luego nos suscribimos
            // Debug.Log(gameObject.name + " suscrito al evento OnPlatformReturn.");
        }
        else
        {
            Debug.LogError("PlatformEventManager no encontrado en la escena.");
        }
    }

    private void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        float speed = isSeparating ? separationSpeed : moveSpeed;
        float direction = movingForward ? 1 : -1;

        transform.position += Vector3.forward * direction * speed * Time.deltaTime;

        // Debug.Log(gameObject.name + " moviéndose en dirección: " + (movingForward ? "adelante" : "atrás"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform")) 
        {
            isSeparating = true; // Se separa al chocar con otra plataforma
            movingForward = !movingForward; // 🔹 Invertimos la dirección de inmediato
            moveSpeed = separationSpeed; // 🔹 Reducimos la velocidad temporalmente

            // Debug.Log(gameObject.name + " colisionó con otra plataforma. Cambiando dirección y reduciendo velocidad.");
        }
        else if (other.CompareTag("ReturnPlatform"))
        {
            isSeparating = false; // 🔹 Ya no estamos separándonos
            moveSpeed = 20f; // 🔹 Restauramos velocidad normal

            // ❌ Eliminamos el cambio de dirección aquí, dejamos que lo haga el evento
            // movingForward = !movingForward; 

            PlatformEventManager.Instance.TriggerPlatformReturn(); // 🔹 Disparamos el evento de sincronización

            // Debug.Log(gameObject.name + " tocó ReturnPlatform. Disparando evento.");
        }
    }

    private void ChangeDirection()
    {
        bool previousDirection = movingForward; // Guardamos la dirección anterior
        movingForward = !movingForward;
        moveSpeed = 20f; // 🔹 Restauramos la velocidad normal

        // Debug.Log(gameObject.name + " CAMBIO DE DIRECCIÓN: de " + (previousDirection ? "adelante" : "atrás") + 
        //         " a " + (movingForward ? "adelante" : "atrás"));
    }


    private void OnDestroy()
    {
        // Nos aseguramos de quitar la suscripción cuando el objeto sea destruido
        if (PlatformEventManager.Instance != null)
            PlatformEventManager.Instance.OnPlatformReturn -= ChangeDirection;
    }
}
