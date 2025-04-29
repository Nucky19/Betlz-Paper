using UnityEngine;

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject screenVersion1;
    [SerializeField] private GameObject screenVersion2;
    [SerializeField] private bool startWithVersion1 = true;

    private bool hasSwitched = false;

    void Start(){
        screenVersion1.SetActive(startWithVersion1);
        screenVersion2.SetActive(!startWithVersion1);
    }

    private void OnTriggerEnter(Collider other){
        if (!hasSwitched && other.CompareTag("Player")){
            hasSwitched = true;

            screenVersion1.SetActive(!startWithVersion1);
            screenVersion2.SetActive(startWithVersion1);
        }
    }
}
