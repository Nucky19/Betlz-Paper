using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpilogoTrigger : MonoBehaviour
{
   [SerializeField] string sceneName;

   void OnTriggerEnter(Collider collider){
        if (collider.gameObject.CompareTag("Player")) SceneManager.LoadScene(sceneName);
   }
}
