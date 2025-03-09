using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CraneArea : MonoBehaviour
{
    public static event Action<bool> OnCraneArea;
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Player")){
            OnCraneArea(true);
        }
    }
    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("Player")){
            OnCraneArea(false);
        }
    }
}
