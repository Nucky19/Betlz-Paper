using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    //Movement
    [SerializeField] private float playerVelConstant = 3f;
    [SerializeField] private float playerVel = 3f;
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
    [SerializeField] private float _FrogVel=2f;
    [SerializeField] private float _FrogJumpForce=15f;
    [SerializeField] private bool _frogJumpComplete =false;
    [SerializeField] private bool _isDeath =false;

    //Jump
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private float _playerVelJump = 3.2f;
    [SerializeField] private float _doubleJumpForce = 4.2f;
    [SerializeField] private float _playerVelDoubleJump = 3.5f;
    private float _bufferTime = 0.25f;
    private float _bufferTimer;
    [SerializeField] private bool _doubleJump =false;
    [SerializeField] private bool _inAir =false;

    //GroundSensor
    [SerializeField] Transform _sensorPosition;
    [SerializeField]  LayerMask _groundLayer;
    [SerializeField]  float _groundSensorX = 0.65f;
    [SerializeField]  float _groundSensorY = 0.5f;
    [SerializeField]  float _groundSensorZ = 0.61f;
    [SerializeField] private float _slideSpeed = 1f;
    [SerializeField] private float _raySize = 1f;

    //Gravity
    [SerializeField] private float  _gravity = -37f;
    [SerializeField] private Vector3 _playerGravity;

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //TODO Hacer array que almacene las dos transformaciones disponibles en ese momento, 
    //para luego en los debidos condicionales comprobar si esta transformado o no a partir de esa arary
    //en vez de ir uno por uno
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    void Start(){
        Application.targetFrameRate = 60; //Capar a 60fps el juego;
    }

    void Awake(){
        characterController = GetComponent<CharacterController>();
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
        InAir(_inAir);
        if(Input.GetKeyDown("z") && IsGrounded() && !_frogJumpComplete){
            if (!_isFrog) FrogTransformation();
            else if(_isFrog) SetNormalState();
        }
        Gravity();
        if(!IsGrounded()) Checkcorner();

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
        Vector3 totalMovement = movement * Time.deltaTime; 
        characterController.Move(totalMovement*playerVel);
    }

    void InAir(bool inAir){
        if (inAir) { 
            if(_doubleJump==true && Input.GetButtonDown("Jump") && !_isFrog){
                _doubleJump=false;
                Jump(_doubleJumpForce);
            }
            
            if(!_doubleJump && Input.GetButtonDown("Jump")) _bufferTimer=_bufferTime;
            _bufferTimer -= Time.deltaTime;

        } else if (_playerGravity.y < 0) {
            _inAir=false;
        
            if (_frogJumpComplete){
                _frogJumpComplete = false; 
                SetNormalState(); 
            }
            _doubleJump=true;
            if (!_isFrog && _doubleJump) playerVel=playerVelConstant;
            if(_bufferTimer>0) Jump(_jumpForce);
        }
    }

   void Gravity(){
    if(!IsGrounded()){  
        _playerGravity.y += _gravity *Time.deltaTime;
        _inAir = true;
    }   
    else if(IsGrounded() && _playerGravity.y <0 ){
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
        return Physics.CheckBox(
        _sensorPosition.position, 
        new Vector3(_groundSensorX, _groundSensorY, _groundSensorZ),     
        Quaternion.identity,      
        _groundLayer.value        
        );
    }
    void Checkcorner(){
        RaycastHit hit;
        if(Physics.Raycast(_sensorPosition.position, transform.forward, out hit, _raySize, _groundLayer) || Physics.Raycast(_sensorPosition.position, -transform.forward, out hit, _raySize, _groundLayer)){
            SlideCorner(hit.normal);
        }
    }
    void SlideCorner(Vector3 slideDirection){
        characterController.Move((slideDirection*_slideSpeed+Vector3.down)*Time.deltaTime);
    }
    void OnDrawGizmos() {
        Vector3 halfExtents = new Vector3(_groundSensorX, _groundSensorY, _groundSensorZ);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_sensorPosition.position, halfExtents * 2); 
    
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_sensorPosition.position, transform.forward*_raySize);
        Gizmos.DrawRay(_sensorPosition.position, -transform.forward*_raySize);
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
    public void Die(){
        _isDeath=true;
        Debug.Log("Death");
    }
}
