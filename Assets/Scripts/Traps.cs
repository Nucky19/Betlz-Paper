using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Traps : MonoBehaviour
{
    public static event Action OnTrapContact;
    private bool isTriggerActive = true;

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.CompareTag("PlayerHitBox") && isTriggerActive){
            isTriggerActive = false;  
            Debug.Log("Jugador tocó una trampa.");
            OnTrapContact?.Invoke(); 
        }
    }

    public void ResetTrap(){
        isTriggerActive = true;  
    }
}
