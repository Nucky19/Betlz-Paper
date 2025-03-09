using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nube_Move : MonoBehaviour
{
    public float speed = 2f; // Velocidad de movimiento de las nubes
    public float resetPositionZ = 50f; // Distancia a la que las nubes se reinician
    public float startPositionZ = -50f; // Posición inicial a la que se mueven al resetearse

    void Update()
    {
        // Mover las nubes en el eje Z positivo
        transform.position += Vector3.forward * speed * Time.deltaTime;

        // Si las nubes superan la posición límite, las reposicionamos atrás
        if (transform.position.z >= resetPositionZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPositionZ);
        }
    }
}