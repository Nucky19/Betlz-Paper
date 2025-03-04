using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movimiento")]
    // [SerializeField] private float speed; // Velocidad de movimiento hacia el choque
    [SerializeField] private float moveSpeed = 20f; // Velocidad de movimiento hacia el choque
    [SerializeField] private float separationSpeed = 3f; // Velocidad al separarse después de chocar
    [SerializeField] private float maxSeparation = 3f; // Distancia máxima de separación antes de volver a moverse al choque
    [SerializeField] private bool movingForward = true; // Si comienza moviéndose hacia adelante en Z

    [Header("Detección de colisión")]
    [SerializeField] private string platformTag = "MovingPlatform"; // Tag de otras plataformas
    [SerializeField] private string playerTag = "Player"; // Tag del jugador
    [SerializeField] private string returnTag = "ReturnPlatform"; // Tag del trigger de retorno
    [SerializeField] private GameObject collisionDetector; // Empty con BoxCollider para detectar colisiones

    private Vector3 startPos;
    private bool isSeparating = false; // Indica si está separándose después de chocar
    private Rigidbody rb;

    private void Start(){
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

        if (rb == null){
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void Update(){
        MovePlatform();
    }

    void MovePlatform() {
        float speed = isSeparating ? separationSpeed : moveSpeed;
        float direction = movingForward ? 1 : -1;

        transform.position += Vector3.forward * direction * speed * Time.deltaTime;

        // Si se está separando, verifica si alcanzó la distancia máxima
        if (isSeparating && Vector3.Distance(transform.position, startPos) >= maxSeparation){
            isSeparating = false;
            movingForward = !movingForward; // Cambia la dirección para moverse otra vez al choque
        }
    }

    private void OnTriggerEnter(Collider other){
    // Si detecta que colisiona con la plataforma
        if (other.CompareTag(platformTag)) {
            isSeparating = true; // Empieza a separarse
            moveSpeed=5f;

        }

        // Verificamos si el objeto es el jugador
        if (other.CompareTag(playerTag)){
            // Verifica si el jugador está atrapado entre las plataformas
            Collider[] colliders = Physics.OverlapBox(collisionDetector.transform.position, collisionDetector.transform.localScale / 2);
            foreach (Collider hit in colliders){
                if (hit.CompareTag(platformTag)){
                    Debug.Log("Muerto");
                }
            }
        }
        
        // Si llega al trigger de retorno
        else if (other.CompareTag(returnTag)){
            startPos = transform.position; // Resetea la posición inicial
            movingForward = !movingForward; // Invierte el movimiento
            moveSpeed=20f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag)){
            collision.transform.parent = this.transform; // Hace que el jugador se mueva con la plataforma
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag)){
            collision.transform.parent = null; // Libera al jugador cuando se baja de la plataforma
        }
    }

    private void OnDrawGizmos()
    {
        if (collisionDetector != null){
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(collisionDetector.transform.position, collisionDetector.transform.localScale);
        }
    }
}
