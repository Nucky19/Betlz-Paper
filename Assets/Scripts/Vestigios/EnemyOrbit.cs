using UnityEngine;

public class EnemyOrbit : MonoBehaviour
{
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private bool rotateClockwise = true;

    private Vector3 initialPosition;

    void Start(){
        // Configura la posición inicial en el radio especificado
        initialPosition = orbitCenter.position + new Vector3(0f, orbitRadius, 0f);
        transform.position = initialPosition;
    }

    void OnEnable(){
        PlayerStates.OnDeath += ResetPosition;
    }

    void OnDisable(){
        PlayerStates.OnDeath -= ResetPosition;
    }

    void Update(){
        // Define la dirección de rotación
        float direction = rotateClockwise ? 1f : -1f;
        transform.RotateAround(orbitCenter.position, Vector3.right, direction * rotationSpeed * Time.deltaTime);
    }

    void ResetPosition(int screen, bool death){
        if (death) transform.position = initialPosition;
    }

    void OnDrawGizmosSelected(){
        if (orbitCenter != null){
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(orbitCenter.position, orbitRadius);
        }
    }
}
