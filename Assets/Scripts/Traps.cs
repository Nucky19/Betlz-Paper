using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Traps : MonoBehaviour
{
    // public PlayerController playerController;
    public static event Action OnTrapContact;  

    
    void Awake(){
        // playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update(){
        
    }
    void OnTriggerEnter(Collider collider){
        // Debug.Log("Trigger Entered");
        if(collider.gameObject.CompareTag("PlayerHitBox")){ 
            Debug.Log("Player Contact");
            OnTrapContact();
        }
    }
}
