using System.Collections;
using System.Collections.Generic;
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
    }

    void Update(){
        if(collectingCrane) GetCrane(isGrounded);
        if(Input.GetKeyDown("f")) OnFrogUnlock?.Invoke();
    }
   
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Player")) {
            switch (gameObject.tag) {
                case "JumpReset":
                    if (!doubleJumpAvaiable) {
                        player._doubleJump = true;
                        StartCoroutine(DisableTemporarily(gameObject, 2f));
                    }
                    break;
                case "Crane":
                    collectingCrane=true;

                    break;
                case "FrogUnlock":
                    OnFrogUnlock?.Invoke();
                    gameObject.SetActive(false);
                    break;
                default:
                    // Destroy(gameObject);
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
