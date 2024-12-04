using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerVel = 3f;
    private float inputHorizontal;
    private float playerRotation;
    private string playerDirection = "right";
    private CharacterController characterController;
    private Vector3 movement;
    
    void Awake(){
        characterController = GetComponent<CharacterController>();
        Application.targetFrameRate = 60; //Capar a 60fps el juego;
    }

    void Update(){
        PlayerMovement();
    }

    void PlayerMovement(){
        inputHorizontal=Input.GetAxisRaw("Horizontal");
        movement.z=inputHorizontal*playerVel;
        if(inputHorizontal>0){
            if(playerDirection=="left"){
                playerRotation = -180f;
                this.transform.Rotate(Vector3.up,playerRotation);
                playerDirection= "right"; 
            } 
            characterController.SimpleMove(movement);
            return;
        }
        if(inputHorizontal<0){
            if(playerDirection=="right"){
                playerRotation = 180f;
                this.transform.Rotate(Vector3.up,playerRotation);
                playerDirection= "left"; 
            } 
            characterController.SimpleMove(movement);
            return;
        }
    }
}
