using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenTrigger : MonoBehaviour
{
    public int screenNumber;  
    public static event Action<int> OnScreen; 

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log("Jugador en pantalla: " + screenNumber);
            OnScreen?.Invoke(screenNumber); 
        }
    }
}
