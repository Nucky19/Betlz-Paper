// using UnityEngine;

// public class PlayerTriggerHandler : MonoBehaviour
// {
//     private Items itemsScript;

//     private void Start()
//     {
//         // Busca el componente Items en el objeto padre (el jugador u otro contenedor)
//         itemsScript = GetComponentInParent<Items>();
//         if (itemsScript == null)
//         {
//             Debug.LogWarning("No se encontró el componente 'Items' en el padre de la hitbox.");
//         }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         itemsScript?.HandleTriggerEnter(other);
//     }
// }
