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
            Debug.LogWarning("No se encontraron Colliders en " + gameObject.name);
        }

        player = FindObjectOfType<PlayerController>();
    }
   
   void OnTriggerEnter(Collider collider) {
    if (collider.gameObject.CompareTag("Player")) {
        switch (gameObject.tag) {
            case "JumpReset":
                if (!doubleJumpAvaiable) {
                    player._doubleJump = true;
                    // Debug.Log("Objeto Adquirido");
                    Destroy(gameObject);
                    //TODO Inhabilitarlo 2 segundos en vez de destruirlo.
                }
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    
}

private int pantalla;

void Test(int numero)
{
    pantalla = numero;
}

void DoubleJump(bool doublejump){
    doubleJumpAvaiable = doublejump;
}



}
