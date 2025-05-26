using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    private bool isPaused;
    private bool pauseAnimation=false;
    [SerializeField] GameObject _pauseCanvas;
    private bool isExit;
    [SerializeField] GameObject _exitCanvas;
    [SerializeField] private Animator _pauseMenuAnimator;
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
    [SerializeField] private Vector3 _spawnPoint;

    public static event Action<string> OnLoadScene;

    private int finalCraneCount;

    void Start()
    {
        if (GlobalGameManager.Instance == null){
            GameObject prefab = Resources.Load<GameObject>("GlobalGameManager");
            if (prefab != null){
                Instantiate(prefab);
                Debug.Log("✅ GlobalGameManager instanciado automáticamente.");
            }else{
                Debug.LogError("❌ No se encontró el prefab GlobalGameManager en Resources.");
            }
        }
        if (SceneManager.GetActiveScene().name == "Final") finalCraneCount = GlobalGameManager.Instance.globalCraneCount;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        hudCanvasGroup.alpha = 1;
        Cursor.visible = false;
        Time.timeScale = 1;
        if (GlobalGameManager.Instance != null && craneText != null) craneText.text = GlobalGameManager.Instance.globalCraneCount.ToString();

        
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        _pauseMenuAnimator = _pauseCanvas.GetComponentInChildren<Animator>();
        // player = GameObject.FindWithTag("Player");
        // Debug.Log(player.name); 
        // characterController = player.GetComponent<CharacterController>();

    }

    void Update()
    {
        if (Input.GetKeyDown("r")) Respawn(currentScreen, true);
        if (Input.GetKeyDown("escape") && SceneManager.GetActiveScene().name == "Prologo") SceneLoad("Level1_Definitivo");

        Debug.Log(GlobalGameManager.Instance.globalCraneCount);
    }

    void OnEnable(){
        ScreenTrigger.OnScreen += HandleCameraChange;
        PlayerStates.OnDeath += Respawn;
        Items.OnFrogUnlock += FrogUnlock;
        Traps.OnTrapContact += ResetTraps;
        PlatformCollisionDetector.OnCollisionContact += ResetCrushing;
        Items.OnCraneCollect += CraneCollect;
        ChangeSpawn.OnChangeSpawn += SpawnChange;
        Inputs.OnPause += Pause;
        // CheckPoint.OnCheckPoint +=   UpdateSpawnPoint;
        // PlayerController.OnIdleStateChanged += HandleHUDVisibility;
        
    }

    void OnDisable(){
        ScreenTrigger.OnScreen -= HandleCameraChange;
        PlayerStates.OnDeath -= Respawn;
        Items.OnFrogUnlock -= FrogUnlock;
        Traps.OnTrapContact -= ResetTraps;
        PlatformCollisionDetector.OnCollisionContact -= ResetCrushing;
        Items.OnCraneCollect -= CraneCollect;
        ChangeSpawn.OnChangeSpawn -= SpawnChange;
        Inputs.OnPause -= Pause;
        // CheckPoint.OnCheckPoint -=   UpdateSpawnPoint;
        // PlayerController.OnIdleStateChanged -= HandleHUDVisibility;
    }

    // void UpdateSpawnPoint(Vector3 spawn)
    // {
    //     _spawnPoint = spawn;
    // }

    public void Pause(){
        if(!isPaused && !pauseAnimation) {
            Cursor.visible = true;
            isPaused=true;
            StartCoroutine(ClosePauseAnimation());
            // Time.timeScale=0;
            _pauseCanvas.SetActive(true);
        }else if(isPaused && !pauseAnimation){
            Cursor.visible = false;
            pauseAnimation=false;
            Time.timeScale=1;
            isPaused = false;
            _pauseCanvas.SetActive(false);
            isExit=true;
            Exit();
        }
    }

    public void Exit(){
        if(!isExit){
            isExit=true;
            _exitCanvas.SetActive(true);
        }else if(isExit){
            isExit=false;
            _exitCanvas.SetActive(false);
        }
    }

    IEnumerator ClosePauseAnimation(){
        _pauseMenuAnimator.SetBool("close",true);
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale=0;
    }

    void ReSpawn(int currentScreen, bool death){
        transform.position = _spawnPoint;
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

    public void RespawnPause(){
        Respawn(currentScreen, true);
        Pause();
    }

    public void Respawn(int screen, bool death){
        Debug.Log($"🔄 Respawn llamado con screen={screen}, death={death}");
        deadCount++;
        GlobalGameManager.Instance.globalDeadCount++;
        deadsText.text=deadCount.ToString();

        if (SceneManager.GetActiveScene().name == "Final")
        {
            finalCraneCount--;
            craneText.text = finalCraneCount.ToString();
            if (deadCount >= GlobalGameManager.Instance.globalCraneCount)
            {
                Debug.Log("☠️☠️☠️☠️☠️☠️☠️☠️☠️☠️☠️kekw");
                player.SetActive(false); // Desactiva al jugador
                return; // No seguimos con el respawn
            }
        }
        if (death && screen >= 0 && screen < respawns.Length)
        {
            if (characterController != null)
            {
                characterController.enabled = false;
                player.transform.position = respawns[screen].position;
                Debug.Log("Posición del jugador: " + player.transform.position);

                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null) playerController.ResetMovement();
                characterController.enabled = true;
            }
            ResetTraps();
            ResetCrushing();
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
    void ResetCrushing(){
        PlatformCollisionDetector[] platformCollisionDetector = FindObjectsOfType<PlatformCollisionDetector>();
        foreach (PlatformCollisionDetector platform in platformCollisionDetector){
            platform.ResetCrush();  
        }
    }

    void FrogUnlock(){
        panelImage.sprite = newHUDFrog;
    }

    void CraneCollect()
    {
        // Debug.Log("Collected");
        GlobalGameManager.Instance.globalCraneCount++;
        // craneCount=GlobalGameManager.Instance.globalCraneCount;
        craneText.text = GlobalGameManager.Instance.globalCraneCount.ToString();
    }

    void SpawnChange(int spawnNum, Vector3 newSpawnPosition){
        if (spawnNum >= 0 && spawnNum < respawns.Length){
            Debug.Log($"Spawn {spawnNum} en: {newSpawnPosition}");
            respawns[spawnNum].position = newSpawnPosition;
            if (spawnNum == 9) _spawnPoint = newSpawnPosition;
        }
    
    }

    public void SceneLoad(string sceneName){
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

}
