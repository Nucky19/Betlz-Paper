using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerVelConstant = 0.15f;
    [SerializeField] private float playerVel = 0.15f;
    private float inputHorizontal;
    private float playerRotation;
    private string playerDirection = "right";
    private CharacterController characterController;
    private Vector3 movement;

    //Models
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject frogModel;

    //States
    [SerializeField] private bool _isFrog =false;
    [SerializeField] private float _FrogVel=0.03f;
    [SerializeField] private float _FrogJumpForce=7f;
    [SerializeField] private bool _frogJumpComplete =false;

    //Jump

    [SerializeField] private float _jumpForce = 2f;
    [SerializeField] private float _playerVelJump = 0.17f;
    [SerializeField] private float _doubleJumpForce = 2.2f;
    [SerializeField] private float _playerVelDoubleJump = 0.22f;
    private float _bufferTime = 0.25f;
    private float _bufferTimer;
    [SerializeField] private bool _doubleJump =false;
    [SerializeField] private bool _inAir =false;

    //GroundSensor

    [SerializeField] Transform _sensorPosition;

    [SerializeField]  LayerMask _groundLayer;
    
    [SerializeField]  float _sensorRadius = 0.5f;


    //Gravity

    [SerializeField] private float  _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //TODO Hacer array que almacene las dos transformaciones disponibles en ese momento, 
    //para luego en los debidos condicionales comprobar si esta transformado o no a partir de esa arary
    //en vez de ir uno por uno
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    void Awake(){
        characterController = GetComponent<CharacterController>();
        Application.targetFrameRate = 60; //Capar a 60fps el juego;
        SetNormalModel();
    }

    void Update(){
        PlayerMovement();

        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            if (_isFrog && !_frogJumpComplete){
                Jump(_FrogJumpForce); 
                _frogJumpComplete = true;
            }
            else if (!_isFrog) Jump(_jumpForce); 
        }
        if(Input.GetKeyDown("z") && IsGrounded() && !_frogJumpComplete){
            if (!_isFrog) FrogTransformation();
            else if(_isFrog) SetNormalState();
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
            _inAir=true;
            //_playerGravity.y += _gravity * Time.deltaTime; 

            if(_doubleJump==true && Input.GetButtonDown("Jump") && !_isFrog){
                _doubleJump=false;
                Jump(_doubleJumpForce);
            }
            
            if(!_doubleJump && Input.GetButtonDown("Jump")) _bufferTimer=_bufferTime;
            _bufferTimer -= Time.deltaTime;

        } else if (_playerGravity.y < 0) {
            // _playerGravity.y = -2f;
            _inAir=false;
        
            if (_frogJumpComplete){
                _frogJumpComplete = false; 
                SetNormalState(); 
            }

            _doubleJump=true;
            if (!_isFrog && _doubleJump) playerVel=playerVelConstant;
            
            if(_bufferTimer>0) Jump(_jumpForce);
        }
        Vector3 totalMovement = movement + _playerGravity * Time.deltaTime;
        characterController.Move(totalMovement);
    }

   void Gravity(){
    if(!IsGrounded())
    {  
        _playerGravity.y += _gravity *Time.deltaTime;
        _inAir = true;
    }   
    else if(IsGrounded() && _playerGravity.y <0 )
    {
        _playerGravity.y = -1;
        _inAir = false;
    }

        characterController.Move(_playerGravity * Time.deltaTime);
   }
   
   
    void Jump(float jumpForce){
        if(!_isFrog && _doubleJump) playerVel=_playerVelJump;
        else if(!_isFrog && !_doubleJump) playerVel=_playerVelDoubleJump;
        _playerGravity.y = Mathf.Sqrt(jumpForce * -2 * _gravity);
        _bufferTimer=0;
    }

    bool IsGrounded(){
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos(){

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensorPosition.position, 0.5f);
    }

    void FrogTransformation(){
        _isFrog=true;
        SetFrogModel();
        playerVel=_FrogVel;
        Debug.Log("IsFrog");
    }
    void SetNormalState(){
        _isFrog = false;
        SetNormalModel();
        playerVel = playerVelConstant; 
    }
    private void SetNormalModel(){
        normalModel.SetActive(true);
        frogModel.SetActive(false);
    }
    private void SetFrogModel()
    {
        normalModel.SetActive(false);
        frogModel.SetActive(true);
    }


    void OnTriggerEnter (Collider collider) 
    {
        if(gameObject.layer == 7)
        {
            Destroy(gameObject);
        }

        

    }
}
