using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] int currentScreen;
    [SerializeField] Transform[] respawns; 
    [SerializeField] GameObject player; 
    [SerializeField] CharacterController characterController;

    void Start(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Awake(){
        // player = GameObject.FindWithTag("Player");
        // Debug.Log(player.name); 
        // characterController = player.GetComponent<CharacterController>();

    }

    void OnEnable(){
        ScreenTrigger.OnScreen += HandleCameraChange;
        PlayerStates.OnDeath += Respawn;
        Traps.OnTrapContact += ResetTraps;
    }

    void OnDisable(){
        ScreenTrigger.OnScreen -= HandleCameraChange;
        PlayerStates.OnDeath -= Respawn;
        Traps.OnTrapContact -= ResetTraps;
    }

    void HandleCameraChange(int cameraNumber){
        Debug.Log("Cambiando a pantalla " + cameraNumber);
        currentScreen = cameraNumber;
    }

    void Respawn(int screen, bool death){
        Debug.Log($"🔄 Respawn llamado con screen={screen}, death={death}");

        if (death && screen >= 0 && screen < respawns.Length){
            
            if (characterController != null){
                characterController.enabled = false;
                player.transform.position = respawns[screen].position;
                Debug.Log("Posición del jugador: " + player.transform.position);
                
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null) playerController.ResetMovement();
                characterController.enabled = true;
            }
        

            ResetTraps(); 
            
        
            PlayerStates playerStates = player.GetComponent<PlayerStates>();
            if (playerStates != null) playerStates.Respawn();
        }
        
    }

void ResetTraps(){
        Traps[] traps = FindObjectsOfType<Traps>();
        foreach (Traps trap in traps){
            trap.ResetTrap();  
        }
    }

}
