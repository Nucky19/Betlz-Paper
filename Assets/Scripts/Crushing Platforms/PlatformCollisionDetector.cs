using UnityEngine;

public class PlatformCollisionDetector : MonoBehaviour
{
    private PlatformController platformController;
    private Transform parentPlatform;
    private bool playerDetection=false;
    private bool platformDetection=false;

    public void SetPlatformController(PlatformController controller, Transform platform)
    {
        platformController = controller;
        parentPlatform = platform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            platformController.HandlePlatformCollision(parentPlatform);
            platformDetection=true;

        }
        if (other.CompareTag("Player"))
        {
            playerDetection=true;
        }

        if (other.CompareTag("ReturnPlatform"))
        {
            PlatformEventManager.Instance.TriggerPlatformReturn();
        }
        CheckForPlayerDeath();
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("MovingPlatform"))
        {
            platformDetection = false;
        }

        if (other.CompareTag("Player"))
        {
            playerDetection = false;
        }
    }

    private void CheckForPlayerDeath()
    {
        if (playerDetection && platformDetection)
        {
            Debug.Log("Muerto");
            // Aquí puedes agregar lógica para reiniciar la escena o hacer un respawn
        }
    }
}
