using UnityEngine;

public class ObjectToggleTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private GameObject collider17;
    [SerializeField] private string triggerTag = "Player";

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (objectToDisable != null) objectToDisable.SetActive(false);
            if (objectToEnable != null) objectToEnable.SetActive(true);
            if (collider17 != null) collider17.SetActive(false);
            // Debug.Log("Cambio de objetos completado");
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag(triggerTag)) Debug.Log("Hola");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (objectToDisable != null) objectToDisable.SetActive(true);
            if (objectToEnable != null) objectToEnable.SetActive(false);
            if (collider17 != null) collider17.SetActive(false);
            // Debug.Log("Cambio de objetos revertido");
        }
    }
}
