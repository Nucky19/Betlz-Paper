using System;
using UnityEngine;

public class ChangeSpawn : MonoBehaviour
{
    public static event Action<int, Vector3> OnChangeSpawn; 
    [SerializeField] int numSpawn; 

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.CompareTag("Player")){
            OnChangeSpawn?.Invoke(numSpawn, transform.position); 
        }
    }
}