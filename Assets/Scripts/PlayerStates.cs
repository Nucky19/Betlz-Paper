using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerStates : MonoBehaviour
{
    public PlayerController player;
    public static event Action<bool> OnDeath;
    [SerializeField] private bool _isDeath =false;
    [SerializeField] public GameObject normalModel;
    [SerializeField] public GameObject frogModel;
    private int ActualScreen;

    void OnEnable(){
        GameManager.OnScreen += InScreen;
    }

    void OnDisable(){
        GameManager.OnScreen -= InScreen;
    }
    void Awake(){
        player = GetComponent<PlayerController>();
        SetNormalModel();
    }
    
    public void SetNormalState(){
        player._isFrog = false;
        SetNormalModel();
        player.playerVel = player.playerVelConstant; 
    }

    public void FrogTransformation(){
        player._isFrog=true;
        SetFrogModel();
        player.playerVel=player._FrogVel;
        Debug.Log("IsFrog");
    }

    private void SetNormalModel(){
        normalModel.SetActive(true);
        frogModel.SetActive(false);
    }

    private void SetFrogModel(){
        normalModel.SetActive(false);
        frogModel.SetActive(true);
    }
    public void Die(){
        _isDeath=true;
        OnDeath(true);
        // Debug.Log("Death");
    }

    void InScreen(int screen){
        ActualScreen=screen;
    }
}
