using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] int currentScreen;
    [SerializeField] Transform[] respawns; // 🔹 Array con los puntos de respawn
    [SerializeField] GameObject player;  // 🔹 Asignamos manualmente el objeto "Player"
    [SerializeField] CharacterController characterController;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Awake()
    {
        // player = GameObject.FindWithTag("Player");
        // Debug.Log(player.name);  // Para asegurarte de que el GameObject correcto está siendo asignado
        // characterController = player.GetComponent<CharacterController>();

    }

    void OnEnable()
    {
        ScreenTrigger.OnScreen += HandleCameraChange;
        PlayerStates.OnDeath += Respawn;
        Traps.OnTrapContact += ResetTraps;
    }

    void OnDisable()
    {
        ScreenTrigger.OnScreen -= HandleCameraChange;
        PlayerStates.OnDeath -= Respawn;
        Traps.OnTrapContact -= ResetTraps;
    }

   void HandleCameraChange(int cameraNumber)
    {
        Debug.Log("Cambiando a pantalla " + cameraNumber);
        currentScreen = cameraNumber;
    }

void Respawn(int screen, bool death)
{
    Debug.Log($"🔄 Respawn llamado con screen={screen}, death={death}");

    if (death && screen >= 0 && screen < respawns.Length)
    {
        Debug.Log("✅ Respawn ejecutándose correctamente.");

        if (characterController  != null)
        {
            characterController.enabled = false;
            player.transform.position = respawns[screen].position;
            Debug.Log("➡️ Nueva posición del jugador: " + player.transform.position);
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ResetMovement();
                Debug.Log("🛑 Movimiento reseteado.");
            }

            characterController.enabled = true;
        }
        else
        {
            Debug.LogError("❌ ERROR: No se encontró CharacterController en el jugador.");
            player.transform.position = respawns[screen].position;
        }
        ResetTraps();
    }
    else
    {
        Debug.LogError("❌ ERROR: Índice de respawn inválido o muerte no detectada correctamente.");
    }
}
void ResetTraps()
    {
        Traps[] traps = FindObjectsOfType<Traps>();
        foreach (Traps trap in traps)
        {
            trap.ResetTrap();  // Reactiva cada trampa
            Debug.Log("🔄 Trampa reseteada.");
        }
    }

}
