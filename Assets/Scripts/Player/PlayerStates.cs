using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerStates : MonoBehaviour
{

    public PlayerController player;
    public static event Action<int, bool> OnDeath;
    public static event Action <int, bool> OnRespawn; 
    public static event Action <int, bool> OnRespawnItem; 
    [SerializeField] private bool _isDeath =false;
    [SerializeField] public GameObject normalModel;
    [SerializeField] public GameObject frogModel;
    [SerializeField] private BoxCollider playerHitbox;
    private int ActualScreen;

    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip frogTransClip;
    [SerializeField] private AudioClip frogClip;

    void OnEnable(){
        ScreenTrigger.OnScreen += InScreen;
        Traps.OnTrapContact += Die;
        PlatformCollisionDetector.OnCollisionContact += Die;
        // ScreenTrigger.OnEnterScreen += InScreen;
    }

    void OnDisable(){
        ScreenTrigger.OnScreen -= InScreen;
        Traps.OnTrapContact -= Die;
        PlatformCollisionDetector.OnCollisionContact -= Die;
        // ScreenTrigger.OnEnterScreen -= InScreen;
    }

    void Awake(){
        player = GetComponent<PlayerController>();
        SetNormalModel();

        // _audio = GetComponent<AudioSource>();
    }
    
    // void OnTriggerEnter(Collider collider){
    //     if(collider.CompareTag("FrogUnlock")) OnFrogUnlock?.Invoke();
    // }

    public void SetNormalState(){
        player._isFrog = false;
        SetNormalModel();
        player.playerVel = player.playerVelConstant;

        CharacterController characterController = player.GetComponent<CharacterController>();

        if (characterController != null){
            characterController.height = 4.19f;
            characterController.center = new Vector3(characterController.center.x, 1.0f, characterController.center.z);
            characterController.radius = 0.37f;
        }

        player._groundSensorX = 0.65f;
        player._groundSensorY = 0.5f;
        player._groundSensorZ = 0.41f;
        player._rayUpSize = 4.15f;
        player._rayDownSize = 0.7f;

        if (playerHitbox != null){
            playerHitbox.size = new Vector3(1.172319f, 4.891592f, 0.9461098f);
            playerHitbox.center = new Vector3(0f, 1.0f, 0f);
        }
    }

    public void FrogTransformation(){
        player._isFrog = true;
        SetFrogModel();
        player.playerVel = player._FrogVel;
        Debug.Log("IsFrog");

        _audio.Stop();
        _audio.PlayOneShot(frogTransClip, 0.7F);
        // Invoke(nameof(PlayFrogClip), frogTransClip.length);
        _audio.Play();

        CharacterController characterController = player.GetComponent<CharacterController>();

        if (characterController != null){
            characterController.height = 0;
            characterController.center = new Vector3(characterController.center.x, -0.42f, characterController.center.z);
            characterController.radius = 0.85f;
        }

        player._groundSensorX = 0.65f;
        player._groundSensorY = 0.5f;
        player._groundSensorZ = 0.41f;
        player._rayUpSize = 2f;
        player._rayDownSize = 0.7f;

        if (playerHitbox != null){
            // Debug.Log("ENTRAAA EN CAMBIO DE HITBOX RANA");x
            playerHitbox.size = new Vector3(3.38f, 1.22f, 0.9461098f);
            playerHitbox.center = new Vector3(0f, -0.68f, 0.26f);
        }
    }

    void PlayFrogClip(){
        _audio.PlayOneShot(frogClip, 0.17F);
        _audio.Play();
    }

    private void SetNormalModel(){
        normalModel.SetActive(true);
        frogModel.SetActive(false);
    }

    private void SetFrogModel(){
        normalModel.SetActive(false);
        frogModel.SetActive(true);
    }

    public void DoubleJumpHitBox(bool OnJump){
        CharacterController characterController = player.GetComponent<CharacterController>();
        if(OnJump){ //DBjumping
            playerHitbox.size = new Vector3(1.172319f, 2.05f, 1.37f);
            playerHitbox.center = new Vector3(0f, 1.0f, 0f);
            player._rayUpSize = 2.86f;
            if (characterController != null){
            characterController.height = 0;
            characterController.center = new Vector3(characterController.center.x, 0.56f, characterController.center.z);
            characterController.radius = 0.37f;
        }

        }else{
            playerHitbox.size = new Vector3(1.172319f, 4.891592f, 0.9461098f);
            playerHitbox.center = new Vector3(0f, 1.0f, 0f);
            player._rayUpSize = 4.15f;
            if (characterController != null){
            characterController.height = 4.19f;
            characterController.center = new Vector3(characterController.center.x, 1.0f, characterController.center.z);
            characterController.radius = 0.37f;
        }
        }
    }

    public void Die(){
        // Debug.Log("AAAAAAAAAAAAAAAAAAAAAA");
        
        if (!_isDeath){
            _audio.Stop();
            _audio.PlayOneShot(deathClip, 0.5F);
            _audio.Play();
            _isDeath = true;
            OnDeath?.Invoke(ActualScreen, true); 
        }
    }

    

    public void Respawn(){
        _isDeath = false;  // Marca que el jugador ha revivido
        OnRespawn?.Invoke(ActualScreen, false);  // Notifica a los suscriptores que el jugador ha respawneado
        SetNormalState();  // Restablece el modelo y el estado normal del jugador

        StartCoroutine(DelayedRespawn());
    }

    IEnumerator DelayedRespawn() {
        // Esperamos 1 segundo
        yield return new WaitForSeconds(1f);
        OnRespawnItem?.Invoke(ActualScreen, false);
    }

    void InScreen(int screen){
        ActualScreen=screen;
    }
}
