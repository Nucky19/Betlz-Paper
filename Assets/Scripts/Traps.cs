using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    // public PlayerController playerController;
    void Awake(){
        // playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update(){
        
    }
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag=="Player"){
            Debug.Log("Player Contact");
            // playerController.Die();
            PlayerController.Instance.Die();
        }
    }
}
