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
        // inputHorizontal=Input.GetAxisRaw("Horizontal");
        float joystickHorizontal = Input.GetAxisRaw("Horizontal");
        float dpadHorizontal = Input.GetAxisRaw("DPadHorizontal");
        float dpadHorizontalAlt = Input.GetAxisRaw("DPadHorizontalAlt");
        inputHorizontal = Mathf.Abs(joystickHorizontal) > 0.1f ? joystickHorizontal : Mathf.Abs(dpadHorizontal) > 0.1f ? dpadHorizontal : dpadHorizontalAlt;
        jump = Input.GetButtonDown("Jump") || 
        Input.GetKeyDown(KeyCode.JoystickButton0) ||  
        Input.GetKeyDown(KeyCode.JoystickButton1) ||  
        Input.GetKeyDown(KeyCode.JoystickButton2) ||  
        Input.GetKeyDown(KeyCode.JoystickButton3); 
        // if(frogAvaiable) firstTransformation=Input.GetKeyDown("z") || Input.GetKeyDown("k");    
        if(frogAvaiable) firstTransformation = frogAvaiable && 
            (Input.GetButtonDown("Transform") || 
            Input.GetKeyDown(KeyCode.JoystickButton6) ||  // ZL
            Input.GetKeyDown(KeyCode.Z) || 
            Input.GetKeyDown(KeyCode.K));  
        if(Input.GetKey("r") && Input.GetKey("q")) SceneManager.LoadScene("MainMenu");
        // if(Input.GetKeyDown(KeyCode.Escape)) OnPause?.Invoke();
        if (Input.GetButtonDown("Pause") && SceneManager.GetActiveScene().name != "Prologo") OnPause?.Invoke();
        if (Input.GetButtonDown("Pause") && SceneManager.GetActiveScene().name == "FinalCredits") SceneManager.LoadScene("MainMenu");
        if (Input.GetButtonDown("Pause") && SceneManager.GetActiveScene().name == "BadEning") SceneManager.LoadScene("MainMenu");
        if (Input.GetButtonDown("Pause") && SceneManager.GetActiveScene().name == "GoodCreditos") SceneManager.LoadScene("MainMenu");
        if(Input.GetKeyDown("3")) SceneManager.LoadScene("Level3");
        if(Input.GetKeyDown("4")){
            GlobalGameManager.Instance.globalCraneCount = 17;  // ✅ Establecer valor antes de cambiar de escena
            SceneManager.LoadScene("Final");
        }
    }
    
    void Update(){
        HandleInput();
        // Debug.Log($"Joystick: {Input.GetAxisRaw("Horizontal")} | DPad6: {Input.GetAxisRaw("DPadHorizontal")} | DPad7: {Input.GetAxisRaw("DPadHorizontalAlt")}");
    }
    void UnlockFrog(){
        frogAvaiable=true;
    }
 
}
