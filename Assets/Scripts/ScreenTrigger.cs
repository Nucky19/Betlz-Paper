using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenTrigger : MonoBehaviour
{
    public int screenNumber;  // 🔹 Número de pantalla al que pertenece este trigger
    public static event Action<int> OnScreen; // 🔹 Evento para notificar en qué pantalla está el jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró en la pantalla: " + screenNumber);
            OnScreen?.Invoke(screenNumber); // 🔹 Disparamos el evento con la pantalla actual
        }
    }
}
