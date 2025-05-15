using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour
{
    public float inputHorizontal;
    public bool jump;
    public bool firstTransformation;
    private bool frogAvaiable=false;
    public static event Action OnPause; 

    void OnEnable(){
        Items.OnFrogUnlock+=UnlockFrog;
    }
    void OnDisable(){
        Items.OnFrogUnlock-=UnlockFrog;
    }
    void HandleInput(){
        inputHorizontal=Input.GetAxisRaw("Horizontal");
        jump=Input.GetButtonDown("Jump");
        if(frogAvaiable) firstTransformation=Input.GetKeyDown("z") || Input.GetKeyDown("k");    
        if(Input.GetKey("r") && Input.GetKey("q")) SceneManager.LoadScene("MainMenu");
        if(Input.GetKeyDown(KeyCode.Escape)) OnPause?.Invoke();

    }
    
    void Update(){
        HandleInput();
    }
    void UnlockFrog(){
        frogAvaiable=true;
    }

    
}
