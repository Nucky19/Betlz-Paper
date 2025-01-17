using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
public class CamaraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

   /* void DeactivateCameraByName(string cam1)
{
    GameObject cameraObject = GameObject.Find("Virtual Camera_Pantalla 1"); // Busca el objeto por nombre
    if (cameraObject != null)
    {
        CinemachineVirtualCamera virtualCamera = cameraObject.GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.enabled = false; // Desactiva la cámara virtual
            Debug.Log($"Cámara {cam1} desactivada.");
        }
    }
}*/


void OnTriggerEnter ( Collider collider)
{

    
        // Verifica que el objeto que entra es el jugador
        if (CompareTag("Player"))
        {
            virtualCamera.Priority = 0; 
        }

}


}

