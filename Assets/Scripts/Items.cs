using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] bool doubleJumpAvaiable;


   void OnEnable(){
        PlayerController.OnPlayerDoubleJump += DoubleJump;
   }

   void OnDisable(){
        PlayerController.OnPlayerDoubleJump -= DoubleJump;
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
   
void OnTriggerEnter(Collider collider) {
    if (collider.gameObject.CompareTag("Player")) {
        Debug.Log("El jugador tocó el objeto: " + gameObject.name);

        switch (gameObject.tag) {
            case "JumpReset":
                if (!doubleJumpAvaiable) {
                    player._doubleJump = true;
                    StartCoroutine(DisableTemporarily(gameObject, 2f));
                }
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}

IEnumerator DisableTemporarily(GameObject obj, float time) {
    Debug.Log("Desactivando: " + obj.name);

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

    Debug.Log("Reactivado: " + obj.name);
}

void DoubleJump(bool doublejump){
    doubleJumpAvaiable = doublejump;
}



}
