using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Traps : MonoBehaviour
{
    public static event Action OnTrapContact;
    [SerializeField]private bool isTriggerActive = true;

    void OnTriggerEnter(Collider collider){
        // if(gameObject.CompareTag("MovingPlatform")) isTriggerActive = false;  
        if (collider.gameObject.CompareTag("PlayerHitBox") && isTriggerActive){
            Debug.Log("Jugador tocó una trampa.");
            isTriggerActive = false;  
            OnTrapContact?.Invoke(); 
        }
    }

    public void ResetTrap(){
        isTriggerActive = true;  
    }
}
