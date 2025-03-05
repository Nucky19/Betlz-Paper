using System;
using UnityEngine;

public class PlatformEventManager : MonoBehaviour
{
    public static PlatformEventManager Instance; // Singleton para acceder desde cualquier parte del código

    public event Action OnPlatformReturn; // Evento que se dispara cuando una plataforma toca el ReturnPlatform

    private bool eventCooldown = false; // 🔹 Para evitar múltiples llamadas en un solo frame

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject); // Asegura que solo haya una instancia
    }

    public void TriggerPlatformReturn()
    {
        if (!eventCooldown) // 🔹 Solo permite una llamada por frame
        {
            eventCooldown = true;
            Debug.Log("Evento disparado: cambiando dirección de las plataformas.");
            OnPlatformReturn?.Invoke(); // Llama al evento si hay suscriptores
        }
    }

    private void LateUpdate()
    {
        eventCooldown = false; // 🔹 Reseteamos el cooldown al final del frame
    }
}
