using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerStates : MonoBehaviour
{

    public PlayerController player;
    public static event Action<int, bool> OnDeath;
    public static event Action OnRespawn; // 🔹 Evento para que otros scripts sepan que el jugador ha reaparecido
    [SerializeField] private bool _isDeath =false;
    [SerializeField] public GameObject normalModel;
    [SerializeField] public GameObject frogModel;
    [SerializeField] private BoxCollider playerHitbox;
    private int ActualScreen;

    void OnEnable(){
        ScreenTrigger.OnScreen += InScreen;
        Traps.OnTrapContact += Die;
        // ScreenTrigger.OnEnterScreen += InScreen;
    }

    void OnDisable(){
        ScreenTrigger.OnScreen -= InScreen;
        Traps.OnTrapContact -= Die;
        // ScreenTrigger.OnEnterScreen -= InScreen;

    }

    void Awake(){
        player = GetComponent<PlayerController>();
        SetNormalModel();
    }
    
    public void SetNormalState()
    {
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

    public void FrogTransformation()
    {
        player._isFrog = true;
        SetFrogModel();
        player.playerVel = player._FrogVel;
        Debug.Log("IsFrog");

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
            playerHitbox.size = new Vector3(3.38f, 1.22f, 2.846218f);
            playerHitbox.center = new Vector3(0f, -0.68f, 0.26f);
        }
    }

    private void SetNormalModel(){
        normalModel.SetActive(true);
        frogModel.SetActive(false);
    }

    private void SetFrogModel(){
        frogModel.SetActive(true);
        normalModel.SetActive(false);
    }
    public void Die()
    {
        if (!_isDeath)
        {
            _isDeath = true;
            Debug.Log("💀 Jugador ha muerto. Activando respawn...");
            OnDeath?.Invoke(ActualScreen, true); // 🔹 Disparamos el evento de muerte con la pantalla actual
        }
        else
        {
            Debug.LogError("⚠️ Jugador intentó morir pero ya estaba en estado de muerte.");
        }
    }

   public void Respawn()
    {
        Debug.Log($"♻️ Jugador reapareciendo... (Antes de reset, _isDeath={_isDeath})");

        _isDeath = false; // 🔹 Asegurar que se reinicia correctamente

        Debug.Log($"✅ Estado de muerte reiniciado (_isDeath={_isDeath})");

        OnRespawn?.Invoke(); // 🔹 Disparamos el evento de respawn para que otros scripts lo sepan
        SetNormalState(); // 🔹 Restauramos el estado normal del jugador
    }

    void InScreen(int screen){
        ActualScreen=screen;
    }
}
