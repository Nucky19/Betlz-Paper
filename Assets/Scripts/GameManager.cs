using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] int currentScreen;
    [SerializeField] Transform[] respawns; 
    [SerializeField] GameObject player; 
    [SerializeField] CharacterController characterController;
    [SerializeField] private Image panelImage;
    [SerializeField] private Sprite newHUDFrog;
    [SerializeField] Text deadsText;
    [SerializeField] Text craneText;
    private int deadCount=0;
    private int craneCount=0;
    private bool frogUnlock=false;

    [SerializeField] private CanvasGroup hudCanvasGroup; 
    

    void Start(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        hudCanvasGroup.alpha = 1; 
    }

    void Awake(){
        // player = GameObject.FindWithTag("Player");
        // Debug.Log(player.name); 
        // characterController = player.GetComponent<CharacterController>();

    }

    void OnEnable(){
        ScreenTrigger.OnScreen += HandleCameraChange;
        PlayerStates.OnDeath += Respawn;
        Items.OnFrogUnlock += FrogUnlock;
        Traps.OnTrapContact += ResetTraps;
        Items.OnCraneCollect += CraneCollect;
        // PlayerController.OnIdleStateChanged += HandleHUDVisibility;
        
    }

    void OnDisable(){
        ScreenTrigger.OnScreen -= HandleCameraChange;
        PlayerStates.OnDeath -= Respawn;
        Items.OnFrogUnlock -= FrogUnlock;
        Traps.OnTrapContact -= ResetTraps;
        Items.OnCraneCollect -= CraneCollect;
        // PlayerController.OnIdleStateChanged -= HandleHUDVisibility;
    }

    private void HandleHUDVisibility(bool isIdle) {
        if (isIdle) {
            StartCoroutine(FadeCanvas(hudCanvasGroup, 0f, 1f, 0.5f)); // Fade in
        } else {
            StartCoroutine(FadeCanvas(hudCanvasGroup, 1f, 0f, 0.5f)); // Fade out
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration) {
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    void HandleCameraChange(int cameraNumber){
        Debug.Log("Cambiando a pantalla " + cameraNumber);
        currentScreen = cameraNumber;
    }

    void Respawn(int screen, bool death){
        Debug.Log($"🔄 Respawn llamado con screen={screen}, death={death}");
        deadCount++;
        deadsText.text=deadCount.ToString();
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

    void FrogUnlock(){
        panelImage.sprite = newHUDFrog;
    }

    void CraneCollect(){
        Debug.Log("Collected");
        craneCount++;
        craneText.text=craneCount.ToString();
    }

}
