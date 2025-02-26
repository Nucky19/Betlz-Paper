using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] int currentScreen;
    [SerializeField] Transform[] respawns;
    GameObject player;

    void Awake(){
        player=GameObject.FindWithTag("Player");
    }

    void OnEnable(){
        CameraController.OnScreen += HandleCameraChange;
        PlayerStates.OnDeath += Respawn;
    }

    void OnDisable(){
        CameraController.OnScreen -= HandleCameraChange;
        PlayerStates.OnDeath -= Respawn;
    }

    void HandleCameraChange(int cameraNumber)
    {
        Debug.Log(cameraNumber);
        currentScreen=cameraNumber;
    }

    void Respawn(int Screen, bool death){   
        player.transform.position=respawns[Screen].position;
    }
}
