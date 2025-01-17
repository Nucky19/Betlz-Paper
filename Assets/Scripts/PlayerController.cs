using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerVel = 3f;
    private float inputHorizontal;
    private float playerRotation;
    private string playerDirection = "right";
    private CharacterController characterController;
    private Vector3 movement;

    //Jump
    private float _jumpForce = 5;

    //GroundSensor

    [SerializeField] Transform _sensorPosition;

    [SerializeField]  LayerMask _groundLayer;
    
    [SerializeField]  float _sensorRadius = 0.5f;


    //Gravity

    [SerializeField] private float  _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    
    void Awake(){
        characterController = GetComponent<CharacterController>();
        Application.targetFrameRate = 60; //Capar a 60fps el juego;
    }

    void Update(){
        PlayerMovement();

        if(Input.GetButtonDown("Jump") && IsGrounded()){
           Jump();
        }

        Gravity();
    }

    void PlayerMovement() {
    
    inputHorizontal = Input.GetAxisRaw("Horizontal");
    movement.z = inputHorizontal * playerVel;
    if (inputHorizontal > 0){
        if (playerDirection == "left") {
            playerRotation = -180f;
            this.transform.Rotate(Vector3.up, playerRotation);
            playerDirection = "right";
        }
    } else if (inputHorizontal < 0) {
        if (playerDirection == "right") {
            playerRotation = 180f;
            this.transform.Rotate(Vector3.up, playerRotation);
            playerDirection = "left";
        }
    }
    bool isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

    if (!isGrounded) {
        _playerGravity.y += _gravity * Time.deltaTime; 
    } else if (_playerGravity.y < 0) {
        _playerGravity.y = -2f; 
    }
    Vector3 totalMovement = movement + _playerGravity * Time.deltaTime;
    characterController.Move(totalMovement);
}



   void Gravity(){
    if(!IsGrounded())
    {
        _playerGravity.y += _gravity *Time.deltaTime;
    }   
    else if(IsGrounded() && _playerGravity.y <0 )
    {
        _playerGravity.y = -1;
    }

        characterController.Move(_playerGravity * Time.deltaTime);
   }
   
   
    void Jump(){
        _playerGravity.y = Mathf.Sqrt(_jumpForce * -2 * _gravity);

        
    }

    bool IsGrounded(){
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }


   
   
   
   
   
   
   
    void OnDrawGizmos(){

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, 0.5f);
    }
}
