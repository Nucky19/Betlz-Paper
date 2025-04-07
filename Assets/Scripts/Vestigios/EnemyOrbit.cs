using UnityEngine;

public class EnemyOrbit : MonoBehaviour
{
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float orbitRadius = 2f;

    private Vector3 initialPosition;

    void Start(){
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
        transform.RotateAround(orbitCenter.position, Vector3.right, rotationSpeed * Time.deltaTime);
    }

    void ResetPosition(int screen, bool death){
        if (death)transform.position = initialPosition;
    }

    void OnDrawGizmosSelected(){
        if (orbitCenter != null){
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(orbitCenter.position, orbitRadius);
        }
    }
}
