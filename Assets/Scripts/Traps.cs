using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public PlayerController playerController;
    void Awake(){
        playerController=GetComponent<PlayerController>();
    }
    void Update(){
        
    }
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.layer==6){
            Debug.Log("Player Contact");
            playerController.Die();
        }
    }
}
