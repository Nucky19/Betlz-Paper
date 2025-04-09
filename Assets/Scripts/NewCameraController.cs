using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using System.Text.RegularExpressions;

public class NewCameraController : MonoBehaviour
{
  public GameObject cameraToActive;

  public GameObject[] cameras;


  void ChangeCamera()
  {
    foreach(GameObject camera in cameras)
    {
        camera.SetActive(false);
    }

    cameraToActive.SetActive(true);
  }








    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
           ChangeCamera();
            
        }
    }

    
}
