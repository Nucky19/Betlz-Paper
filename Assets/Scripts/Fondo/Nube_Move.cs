using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Nube_Move : MonoBehaviour
{
    // public float speed = 2f; // Velocidad de movimiento de las nubes
    // public float resetPositionZ = 50f; // Distancia a la que las nubes se reinician
    // public float startPositionZ = -50f; // Posición inicial a la que se mueven al resetearse

    // void Update()
    // {
    //     // Mover las nubes en el eje Z positivo
    //     transform.position += Vector3.forward * speed * Time.deltaTime;

    //     // Si las nubes superan la posición límite, las reposicionamos atrás
    //     if (transform.position.z >= resetPositionZ)
    //     {
    //         transform.position = new Vector3(transform.position.x, transform.position.y, startPositionZ);
    //     }
    // }
   public float speed = 2f;
    public float resetPositionZ = 50f;
    public float startPositionZ = -50f;
    public float fadeDistance = 10f; // distancia en la que empieza el fade

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // Mover nubes hacia adelante en el eje Z
        transform.position += Vector3.forward * speed * Time.deltaTime;

        float z = transform.position.z;
        float alpha = 1f;

        // Fade-out al llegar al límite derecho
        if (z >= resetPositionZ - fadeDistance)
        {
            alpha = Mathf.InverseLerp(resetPositionZ, resetPositionZ - fadeDistance, z);
        }
        // Fade-in al aparecer desde el límite izquierdo
        else if (z <= startPositionZ + fadeDistance)
        {
            alpha = Mathf.InverseLerp(startPositionZ, startPositionZ + fadeDistance, z);
        }

        // Aplicar opacidad
        if (spriteRenderer != null)
        {
            Color newColor = originalColor;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }

        // Reiniciar la nube al pasar el límite
        if (z >= resetPositionZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPositionZ);
        }
    }

}