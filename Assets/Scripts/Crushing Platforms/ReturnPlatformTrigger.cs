using UnityEngine;

public class ReturnPlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            PlatformEventManager.Instance.TriggerPlatformReturn();
        }
    }
}
