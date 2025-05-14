using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ScreenTrigger : MonoBehaviour
{
    public int screenNumber;  
    public static event Action<int> OnScreen; 

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(screenNumber==6 && SceneManager.GetActiveScene().name == "Level3" && gameObject.CompareTag("175")) Debug.Log("Sonido"); //reproducir Sonido;
            Debug.Log("Jugador en pantalla: " + screenNumber);
            OnScreen?.Invoke(screenNumber); 
        }
    }
}
