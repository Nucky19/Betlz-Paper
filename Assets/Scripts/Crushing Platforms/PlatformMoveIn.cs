using UnityEngine;
using System.Collections.Generic;

public class PlatformMoveIn : MonoBehaviour
{
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Calcula cuánto se ha movido la plataforma desde el último frame
        Vector3 movement = transform.position - lastPosition;
        lastPosition = transform.position;

        // Si hay jugadores en la plataforma, móverlos con ella
        foreach (GameObject player in playersOnPlatform)
        {
            player.transform.position += movement;
        }
    }

    private List<GameObject> playersOnPlatform = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlatform.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlatform.Remove(other.gameObject);
        }
    }
}
