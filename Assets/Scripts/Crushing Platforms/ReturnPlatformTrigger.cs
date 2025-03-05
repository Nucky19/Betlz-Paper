using UnityEngine;

public class ReturnPlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform")) // Si una plataforma entra en contacto
        {
            PlatformEventManager.Instance.TriggerPlatformReturn();
        }
    }
}
