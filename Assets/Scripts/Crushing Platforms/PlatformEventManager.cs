using System;
using UnityEngine;

public class PlatformEventManager : MonoBehaviour
{
    public static PlatformEventManager Instance;
    public event Action OnPlatformReturn; 

    private bool eventCooldown = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TriggerPlatformReturn()
    {
        if (!eventCooldown)
        {
            eventCooldown = true;
            // Debug.Log("Evento disparado: cambiando dirección de las plataformas.");
            OnPlatformReturn?.Invoke(); 
        }
    }

    private void LateUpdate()
    {
        eventCooldown = false; 
    }
}
