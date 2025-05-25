using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class Items : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] bool doubleJumpAvaiable;
    [SerializeField] bool isGrounded;
    private bool collectingCrane=false;
    [SerializeField] private bool inCraneArea=false;
    public static event Action OnCraneCollect; 
    public static event Action OnFrogUnlock; 
    public GameObject playerObject;
    public GameObject crane;
    public float speed;
    [SerializeField] public bool dead=false;
    private Vector3 initialCranePosition;

    //SFX

    [SerializeField] AudioClip sonidoGrulla;

    [SerializeField] AudioClip sonidoGrullaObtencionDef;

    [SerializeField] AudioClip sonidoRecoleccionRana;



    [SerializeField] AudioClip sonidoMariposa;

    [SerializeField] float disableTimeMariposa = 1.25f;
    [SerializeField] private Animator mariposaAnimator;

    // [SerializeField] private AudioSource _audio;


    //ParticulasDobleSalto


    void OnEnable(){
        PlayerController.OnPlayerDoubleJump += DoubleJump;
        PlayerController.OnGround += Grounded;
        CraneArea.OnCraneArea += InCraneArea;
        PlayerStates.OnDeath += IsDeath;
        PlayerStates.OnRespawnItem += Respawn;

   }

   void OnDisable(){
        PlayerController.OnPlayerDoubleJump -= DoubleJump;
        PlayerController.OnGround -= Grounded;
        CraneArea.OnCraneArea -= InCraneArea;
        PlayerStates.OnDeath -= IsDeath;
        PlayerStates.OnRespawnItem -= Respawn;
    }
    void Start() {
        if (dead) dead = false;  
        }
    void Awake() {
        Collider[] colliders = GetComponents<Collider>();
        if (colliders.Length > 0) {
            foreach (Collider col in colliders) {
                col.isTrigger = true;
            }
        } else {
            Debug.LogWarning("No Colliders en " + gameObject.name);
        }

        player = FindObjectOfType<PlayerController>();
        if (crane != null) initialCranePosition = crane.transform.position;

        mariposaAnimator = GetComponent<Animator>();
        // _audio = GetComponent<AudioSource>();
   
    }


    

    void Update(){
        if(collectingCrane) GetCrane(isGrounded);
        if(Input.GetKeyDown("f")) OnFrogUnlock?.Invoke();
        if (SceneManager.GetActiveScene().name == "Level3" || SceneManager.GetActiveScene().name == "Final") OnFrogUnlock?.Invoke();
    }
   

//     public void HandleTriggerEnter(Collider collider) {
//         if (!collider.gameObject.CompareTag("PlayerHitbox")) return;
//  // Evita autocolisión
        
//         switch (collider.tag) {
//             case "JumpReset":
//                 if (!doubleJumpAvaiable) {
//                     player._doubleJump = true;
//                     AudioSource.PlayClipAtPoint(sonidoMariposa, transform.position);
//                     StartCoroutine(DisableTemporarily(collider.gameObject, disableTimeMariposa));
//                 }
//                 break;
//             case "JumpResetB":
//                 if (!doubleJumpAvaiable) {
//                     player._doubleJump = true;
//                     AudioSource.PlayClipAtPoint(sonidoMariposa, transform.position);
//                     mariposaAnimator.SetTrigger("isBlue");
//                     StartCoroutine(DisableTemporarilyHitBox(collider.gameObject, disableTimeMariposa));
//                 }
//                 break;
//             case "Crane":
//                 collectingCrane = true;
//                 AudioSource.PlayClipAtPoint(sonidoGrulla, transform.position);
//                 break;
//             case "FrogUnlock":
//                 AudioSource.PlayClipAtPoint(sonidoRecoleccionRana, transform.position);
//                 OnFrogUnlock?.Invoke();
//                 collider.gameObject.SetActive(false);
//                 break;
//             default:
//                 break;
//         }
//     }
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("ItemHitBox")) {
            switch (gameObject.tag) {
                // case "JumpReset":
                //     if (!doubleJumpAvaiable) {
                //         player._doubleJump = true;
                //         AudioSource.PlayClipAtPoint(sonidoMariposa, transform.position); //Sonido
                //         StartCoroutine(DisableTemporarily(gameObject, disableTimeMariposa));
                //     }
                //     break;
                case "JumpResetB":
                    if (!doubleJumpAvaiable) {
                        player._doubleJump = true;
                        AudioSource.PlayClipAtPoint(sonidoMariposa, transform.position); //Sonido
                        mariposaAnimator.SetTrigger("isBlue");
                        StartCoroutine(DisableTemporarilyHitBox(gameObject, disableTimeMariposa));
                    }
                    break;
                case "Crane":
                    collectingCrane=true;
                    AudioSource.PlayClipAtPoint(sonidoGrulla, transform.position); //Sonido

                    break;
                case "FrogUnlock":
                    AudioSource.PlayClipAtPoint(sonidoRecoleccionRana, transform.position); //Sonido
                    OnFrogUnlock?.Invoke();
                    gameObject.SetActive(false);
                    break;
                default:
                    // Destroy(gameObject);
                    break;
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("ItemHitBox"))
        {
            switch (gameObject.tag)
            {
                case "JumpReset":
                    if (!doubleJumpAvaiable)
                    {
                        player._doubleJump = true;
                        AudioSource.PlayClipAtPoint(sonidoMariposa, transform.position); //Sonido
                        StartCoroutine(DisableTemporarily(gameObject, disableTimeMariposa));
                    }
                    break;
            }
        }
    }
    void GetCrane(bool ground){
        // Debug.Log("Craned");
        if (dead){
            crane.transform.position = initialCranePosition;  // Vuelve a la posición original
            collectingCrane = false;  // Detenemos el proceso de seguir al jugador
            dead=false;
            return;
        }
        crane.transform.position=Vector3.MoveTowards(crane.transform.position, playerObject.transform.position, speed);
        if(ground && !inCraneArea){
            Debug.Log("Craned 2");
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(sonidoGrullaObtencionDef, transform.position); //Sonido
            collectingCrane=false;
            OnCraneCollect?.Invoke();
            dead=false;
            
        }
    }

    IEnumerator DisableTemporarily(GameObject obj, float time) {

        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        SkinnedMeshRenderer skinnedRenderer = obj.GetComponent<SkinnedMeshRenderer>();

        if (meshRenderer == null) meshRenderer = obj.GetComponentInChildren<MeshRenderer>();
        if (skinnedRenderer == null) skinnedRenderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();
        
        Collider col = obj.GetComponent<Collider>();
        if (col == null) col = obj.GetComponentInChildren<Collider>();
        

        if (meshRenderer) meshRenderer.enabled = false;
        if (skinnedRenderer) skinnedRenderer.enabled = false;
        if (col) col.enabled = false;

        yield return new WaitForSeconds(time); 

    
        if (meshRenderer) meshRenderer.enabled = true;
        if (skinnedRenderer) skinnedRenderer.enabled = true;
        if (col) col.enabled = true;

    }
    IEnumerator DisableTemporarilyHitBox(GameObject obj, float time) {

        Collider col = obj.GetComponent<Collider>();
        if (col == null) col = obj.GetComponentInChildren<Collider>();

        if (col) col.enabled = false;

        yield return new WaitForSeconds(time);

        if (col) col.enabled = true;

    }

    void DoubleJump(bool doublejump){
        doubleJumpAvaiable = doublejump;
    }
    void Grounded(bool ground){
        isGrounded = ground;
    }
    void InCraneArea(bool craneArea){
        inCraneArea = craneArea;
    }
    void IsDeath(int screen, bool death){
        dead=death;
    }
    public void Respawn(int screen, bool death){
        // Debug.Log("Entra a respawn");
        dead=death;
        // Debug.Log(dead);
    }
    // public void  SetDeadFalse() {
    //     dead = false;
    // }


    

}
