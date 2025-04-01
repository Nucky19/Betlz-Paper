using System;
using UnityEngine;

public class ChangeSpawn : MonoBehaviour
{
    public static event Action<int, Vector3> OnChangeSpawn; // Pasamos número y posición
    [SerializeField] int numSpawn; // Número de respawn asociado a este trigger

    void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.CompareTag("Player")) 
        {
            Debug.Log($"📍 Trigger activado en Spawn {numSpawn} - Nueva Posición: {transform.position}");
            OnChangeSpawn?.Invoke(numSpawn, transform.position); // Enviar número y posición
        }
    }
}