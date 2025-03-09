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


   void OnEnable(){
        PlayerController.OnPlayerDoubleJump += DoubleJump;
        PlayerController.OnGround += Grounded;
        CraneArea.OnCraneArea += InCraneArea;
   }

   void OnDisable(){
        PlayerController.OnPlayerDoubleJump -= DoubleJump;
        PlayerController.OnGround -= Grounded;
        CraneArea.OnCraneArea -= InCraneArea;
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
    }

    void Update(){
        if(collectingCrane) GetCrane(isGrounded);
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
                default:
                    Destroy(gameObject);
                    break;
            }
        }
    }

    void GetCrane(bool ground){
        // Debug.Log("Craned");
        if(ground && !inCraneArea){
            Debug.Log("Craned 2");
            gameObject.SetActive(false);
            collectingCrane=false;
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

}
