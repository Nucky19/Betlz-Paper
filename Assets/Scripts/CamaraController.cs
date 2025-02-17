using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using System;
public class CamaraController : MonoBehaviour
{
    public static event Action<int>  OnPlayerTrigger;

    [SerializeField] private CinemachineVirtualCamera[] cameras;

 private int currentCameraIndex = 0; // Índice de la cámara actual

    private void Start()
    {
        // Asegúrate de que la primera cámara sea la activa al inicio
        //SetCameraPriority(currentCameraIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerTrigger(1);

        // Asegúrate de que el objeto que entra en el collider es tu personaje
        if (other.CompareTag("Player"))
        {
            // Determina la dirección de entrada
            Vector3 direction = other.transform.position - transform.position;

            if (direction.x < 0) // Entró por la izquierda
            {
                SwitchToNextCamera();
            }
            else if (direction.x > 0) // Entró por la derecha
            {
                SwitchToPreviousCamera();
            }
        }
    }

    private void SwitchToNextCamera()
    {
        // Cambia a la siguiente cámara
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length; // Asegúrate de que el índice no exceda el tamaño del arreglo
        SetCameraPriority(currentCameraIndex);
    }

    private void SwitchToPreviousCamera()
    {
        // Cambia a la cámara anterior
        currentCameraIndex = (currentCameraIndex - 1 + cameras.Length) % cameras.Length; // Asegúrate de que el índice no sea negativo
        SetCameraPriority(currentCameraIndex);
    }

    private void SetCameraPriority(int index)
    {
        // Establece la prioridad de las cámaras
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = (i == index) ? 1 : 0; // La cámara activa tiene prioridad 1, las demás 0
        }
    }


}

