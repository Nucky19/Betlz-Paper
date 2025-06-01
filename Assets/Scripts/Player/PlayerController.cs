using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    public Inputs input;
    [SerializeField] bool canMove;
    public PlayerStates state;
    private CharacterController characterController;

    //Movement
    [SerializeField] public float playerVelConstant = 3f;
    [SerializeField] public float playerVel = 3f;
    // private float inputHorizontal;
    private float playerRotation;
    private string playerDirection = "right";

    private Vector3 movement;

    //Models
    // [SerializeField] private GameObject normalModel;
    // [SerializeField] private GameObject frogModel;

    //States
    public static event Action<bool> OnFrog;
    
    [SerializeField] public bool _isFrog =false;
    [SerializeField] public float _FrogVel=2.7f;
    [SerializeField] private float _FrogJumpForce=15f;
    [SerializeField] private bool _frogJumpComplete =false;

    //Jump
    public static event Action<bool> OnPlayerDoubleJump;
    [SerializeField] private float _jumpForce = 4f;
    [SerializeField] private float _playerVelJump = 3.2f;
    [SerializeField] private float _doubleJumpForce = 4.2f;
    [SerializeField] private float _playerVelDoubleJump = 3.5f;
    private float _bufferTime = 0.25f;
    private float _bufferTimer;
    [SerializeField] public bool _doubleJump =true;
    [SerializeField] private bool _inAir =false;

    //GroundSensor
    public static event Action<bool> OnGround;
    [SerializeField] Transform _sensorPosition;
    [SerializeField]  LayerMask _groundLayer;
    [SerializeField]  public float _groundSensorX = 0.65f;
    [SerializeField]  public float _groundSensorY = 0.5f;
    [SerializeField]  public float _groundSensorZ = 0.61f;
    [SerializeField] private float _slideSpeed = 1f;
    [SerializeField] public float _raySideSize = 1f;
    [SerializeField] public float _rayUpSize = 4.15f;
    [SerializeField] public float _rayDownSize = 0.7f;

    //Gravity
    [SerializeField] private float  _gravity = -37f;
    [SerializeField] private Vector3 _playerGravity;

    [SerializeField] private float _coyoteTime = 0.15f; 
    private float _coyoteTimer;

    //Animations
    private Animator _animator;
    private float idleTime = 0f;
    private bool isIdle = false;

    //SFX
    
   [SerializeField] private AudioSource _audio;
   [SerializeField] private AudioClip pasosClip;
   [SerializeField] private AudioClip landingClip;
   [SerializeField] private AudioClip jumpClip;
   [SerializeField] private AudioClip doubleJumpClip;

    [SerializeField] private AudioClip frogJumpClip;


     private bool wasGrounded = true;


     //Particles

     public JumpParticles jumpParticles;

   

    
    
    public static event Action<bool> OnIdleStateChanged;

    // private bool frogAvaiable=false;
    // void OnEnable(){
    //     PlayerStates.OnFrogUnlock+=UnlockFrog();
    // }
    // void OnDisable(){
    //     PlayerStates.OnFrogUnlock-=UnlockFrog();
    // }
    [SerializeField] private AudioSource _movementAudio;
    [SerializeField] private AudioSource _jumpAudio;
    void Start(){
        Application.targetFrameRate = 60; //Capar a 60fps el juego;
    }

    void Awake(){
        if (Instance == null) Instance = this;
        else   Destroy(gameObject);
        if (!canMove) canMove = true;
        input = GetComponent<Inputs>();
        state=GetComponent<PlayerStates>();

        characterController = GetComponent<CharacterController>();

        _animator = GetComponentInChildren<Animator>();

    //    _audio = GetComponent<AudioSource>(); //Audio
    //    _movementAudio = GetComponent<AudioSource>(); //Audio
    //    _jumpAudio = GetComponent<AudioSource>(); //Audio

        
        
    }
    public void SetAnimator(Animator newAnimator){
        if (newAnimator != null)_animator = newAnimator;
    }

    void OnEnable()
    {
        Items.OnFrogTutorial += CanMove;
        
    }
    void OnDisable(){
        Items.OnFrogTutorial -= CanMove;
    }

    void CanMove(bool movementAviable){
        canMove = movementAviable;
    }

        void Update(){
        if(canMove) PlayerMovement();
        if (input.jump && IsGrounded()) { 
            if (_audio.isPlaying && _audio.clip == pasosClip)_audio.Stop();  
            
            _jumpAudio.PlayOneShot(jumpClip, 0.7F);//audio
            
            jumpParticles.PlayJumpEffect(); //Particulas
            
            
            if (_isFrog && !_frogJumpComplete){
                Jump(_FrogJumpForce); 
                _jumpAudio.PlayOneShot(frogJumpClip, 0.7F);
                _frogJumpComplete = true;
                
            }
            else if (!_isFrog) Jump(_jumpForce); 
        }
        InAir(_inAir);
        if(input.firstTransformation && IsGrounded() && !_frogJumpComplete){
            if (!_isFrog) state.FrogTransformation();
            else if(_isFrog) state.SetNormalState();
        }
        CheckMovement();
        Gravity();
        if(!IsGrounded()) Checkcorner();
        if(!IsGrounded()) CheckRoof();
        CheckPassablePlatform();

        ReproducirSonidos(); //Audio
    }

    void PlayerMovement() {
    
        // input.inputHorizontal = Input.GetAxisRaw("Horizontal");
        // movement.x = 0;  
        if (Mathf.Abs(transform.position.x) > 0.01f)  transform.position = new Vector3(0, transform.position.y, transform.position.z);
        movement.z = input.inputHorizontal * playerVel;
        
        if (input.inputHorizontal > 0){
            _animator.SetBool("IsRunning", true);
           

            if (playerDirection == "left") {
                playerRotation = -180f;
                this.transform.Rotate(Vector3.up, playerRotation);
                playerDirection = "right";
                
            }
        } else if (input.inputHorizontal < 0) {
            _animator.SetBool("IsRunning", true);
            

            if (playerDirection == "right") {
                playerRotation = 180f;
                this.transform.Rotate(Vector3.up, playerRotation);
                playerDirection = "left";
                
                
            }
        }else if(input.inputHorizontal==0) _animator.SetBool("IsRunning", false); 
        
      

        Vector3 totalMovement = movement * Time.deltaTime; 
        characterController.Move(totalMovement*playerVel);
    }

    void InAir(bool inAir){
        if (inAir) { 
            if(_doubleJump==true && input.jump && !_isFrog){
                jumpParticles.PlayJumpEffect(); //Particulas
                _animator.SetTrigger("IsDoubleJumping");
                OnPlayerDoubleJump(false);
                state.DoubleJumpHitBox(true);
                _doubleJump=false;
                Jump(_doubleJumpForce);
            }
            
            if(!_doubleJump && Input.GetButtonDown("Jump")) _bufferTimer=_bufferTime;
            _bufferTimer -= Time.deltaTime;

        } else if (_playerGravity.y < 0) {
            // _animator.SetBool("IsDoubleJumping",false);
            _animator.SetBool("FreeFall",true);
            // _animator.SetBool("InAirFrog",true);
            _inAir=false;
        
            if (_frogJumpComplete){
                _frogJumpComplete = false; 
                state.SetNormalState(); 
            }
            _doubleJump=true;
            OnPlayerDoubleJump(true);

            if (!_isFrog && _doubleJump) {
                playerVel=playerVelConstant;
                state.DoubleJumpHitBox(false);
            }
            if(_bufferTimer>0) Jump(_jumpForce);
        }
    }

   void Gravity(){
    if(!IsGrounded()){  
        _playerGravity.y += _gravity *Time.deltaTime;
        _inAir = true;
        OnGround(false);
    }   
    else if(IsGrounded() && _playerGravity.y <0 ){
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsGrounded", true);
        _animator.SetBool("FreeFall",false);

        _playerGravity.y = -1;
        _inAir = false;
        OnGround(true);
    }

        characterController.Move(_playerGravity * Time.deltaTime);
   }

    void Jump(float jumpForce){
        if(_doubleJump) {
            _animator.SetBool("IsJumping", true);
            //_animator.SetBool("IsJumpingFrog", true;)
            if (jumpClip != null) _jumpAudio.PlayOneShot(jumpClip);
        }if(!_isFrog && _doubleJump) playerVel=_playerVelJump;
        
        //hace el doble salto
        else if(!_isFrog && !_doubleJump) {
            playerVel=_playerVelDoubleJump;
           if (doubleJumpClip != null) _jumpAudio.PlayOneShot(doubleJumpClip);
        }
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

    void CheckMovement(){
        if (input.inputHorizontal == 0 && Mathf.Abs(characterController.velocity.y) < 0.01f && characterController.velocity.magnitude < 0.01f) {
        idleTime += Time.deltaTime;
        if (idleTime >= 1f && !isIdle) {
            isIdle = true;
            OnIdleStateChanged?.Invoke(true); // Notificar que está quieto
        }
        } else {
            idleTime = 0f;
            if (isIdle) {
                isIdle = false;
                OnIdleStateChanged?.Invoke(false); // Notificar que se está moviendo
            }
        }
    }
    void Checkcorner(){
        RaycastHit hit;
        if(Physics.Raycast(_sensorPosition.position, transform.forward, out hit, _raySideSize, _groundLayer) || Physics.Raycast(_sensorPosition.position, -transform.forward, out hit, _raySideSize, _groundLayer)){
            SlideCorner(hit.normal);
        }
    }

    void SlideCorner(Vector3 slideDirection){
        characterController.Move((slideDirection*_slideSpeed+Vector3.down)*Time.deltaTime);
    }

    void CheckRoof() {
        RaycastHit hit;
        if (Physics.Raycast(_sensorPosition.position, Vector3.up, out hit, _rayUpSize, _groundLayer) && !hit.collider.CompareTag("Passable"))  ApplyCeilingHit();
    }

    void ApplyCeilingHit() {
        if (_playerGravity.y > 0)   _playerGravity.y = -_slideSpeed * 2;
    }

    void CheckPassablePlatform(){
        RaycastHit hit;
        bool platformDetected = false;

        if (Physics.Raycast(_sensorPosition.position, Vector3.down, out hit, _rayDownSize, _groundLayer)){
            if (hit.collider.CompareTag("Passable")) {
                SetPlatformTrigger(hit.collider, false);
                platformDetected = true;
            }
        }   
        
        if (!platformDetected) SetAllPassablePlatformsToTrigger();
    }

    void SetPlatformTrigger(Collider platformCollider, bool isTrigger){
        platformCollider.isTrigger = isTrigger;
    }

    void SetAllPassablePlatformsToTrigger(){
        GameObject[] passablePlatforms = GameObject.FindGameObjectsWithTag("Passable");
        foreach (GameObject platform in passablePlatforms){
            Collider collider = platform.GetComponent<Collider>();
            if (collider != null) collider.isTrigger = true;
        }
    }

    public void ResetMovement()
    {
        movement = Vector3.zero; 
        _playerGravity = Vector3.zero; 
        transform.position = new Vector3(1, transform.position.y, transform.position.z);
        _doubleJump=true;
        _inAir=false;
        OnPlayerDoubleJump(true);
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsGrounded", true);
        _animator.SetBool("FreeFall",false);
    }

    // void UnlockFrog(){
    //     frogAvaiable=true;
    // }
    void OnDrawGizmos() {
        Vector3 halfExtents = new Vector3(_groundSensorX, _groundSensorY, _groundSensorZ);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_sensorPosition.position, halfExtents * 2); 
    
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_sensorPosition.position, transform.forward*_raySideSize);
        Gizmos.DrawRay(_sensorPosition.position, -transform.forward*_raySideSize);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_sensorPosition.position, Vector3.up*_rayUpSize);
        Gizmos.DrawRay(_sensorPosition.position, Vector3.down*_rayDownSize);
    }

    void ReproducirSonidos() //Audio
{
    // Sonido al caminar
    if (IsGrounded() && Mathf.Abs(input.inputHorizontal) > 0.1f)
        {
            if (!_movementAudio.isPlaying || _movementAudio.clip != pasosClip)
            {
                _movementAudio.clip = pasosClip;
                _movementAudio.loop = true;
                _movementAudio.Play();
            }
        }
        else if (_movementAudio.clip == pasosClip && _movementAudio.isPlaying)
        {
            _movementAudio.Stop();
        }

        // Sonido al aterrizar
        if (!wasGrounded && IsGrounded())
        {
            if (landingClip != null) _movementAudio.PlayOneShot(landingClip);
        }

        wasGrounded = IsGrounded();
    
}


   
}
