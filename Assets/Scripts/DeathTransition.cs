using UnityEngine;
using System.Collections;

public class DeathTransition : MonoBehaviour
{
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private Animator deathPanelAnimator;
    [SerializeField] private float blackScreenDuration = 0.4f;

    void OnEnable(){
        PlayerStates.OnRespawn += HandleRespawn;
    }

    void OnDisable(){
        PlayerStates.OnRespawn -= HandleRespawn;
    }
    
    void HandleRespawn(int screen, bool death){
        deathPanelAnimator.SetTrigger("PlayTransition");
    }

    // IEnumerator PlayDeathTransition(){

    //     // yield return new WaitForSeconds(blackScreenDuration);

    //     deathPanelAnimator.SetTrigger("PlayTransition");
    //     // yield return new WaitForSeconds(0.55f);
    //     // deathCanvas.SetActive(false);
    // }
}
