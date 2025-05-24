using System.Collections;
using UnityEngine;

public class Cuchilla : MonoBehaviour
{
    public enum PatrolAxis { Vertical, Horizontal }

    [Header("Patrol Settings")]
    [SerializeField] private Transform pointB; // Solo usaremos puntoB como destino
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool startFromPointB = false; // Ya no es relevante, pero lo dejamos por compatibilidad

    [Header("Reset Trigger")]
    [SerializeField] private GameObject resetTriggerObject;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Coroutine patrolRoutine;

    void OnEnable()
    {
        PlayerStates.OnDeath += HandlePlayerDeath;

        if (resetTriggerObject != null){
            TriggerListener listener = resetTriggerObject.GetComponent<TriggerListener>();
            if (listener == null)
                listener = resetTriggerObject.AddComponent<TriggerListener>();

            listener.OnPlayerEnterTrigger += HandlePlayerEnteredResetTrigger;
        }

        StartMovement();
    }

    void OnDisable(){
        PlayerStates.OnDeath -= HandlePlayerDeath;

        if (resetTriggerObject != null){
            TriggerListener listener = resetTriggerObject.GetComponent<TriggerListener>();
            if (listener != null)
                listener.OnPlayerEnterTrigger -= HandlePlayerEnteredResetTrigger;
        }

        if (patrolRoutine != null) StopCoroutine(patrolRoutine);
    }

    private void HandlePlayerDeath(int screen, bool death){
        if (!death) return;

        if (resetTriggerObject != null && !resetTriggerObject.activeSelf)
            resetTriggerObject.SetActive(true);

        ResetAndStart();
    }

    private void HandlePlayerEnteredResetTrigger(){
        if (resetTriggerObject != null)
            resetTriggerObject.SetActive(false);

        ResetAndStart();
    }

    private void ResetAndStart(){
        if (patrolRoutine != null) StopCoroutine(patrolRoutine);
        transform.position = startPosition;
        transform.rotation = startRotation;
        patrolRoutine = StartCoroutine(MoveToPointB());
    }

    private void StartMovement(){
        startPosition = transform.position;
        startRotation = transform.rotation;
        patrolRoutine = StartCoroutine(MoveToPointB());
    }

    IEnumerator MoveToPointB(){
        while (Vector3.Distance(transform.position, pointB.position) > 0.05f){
            Vector3 direction = (pointB.position - transform.position).normalized;
            float step = speed * Time.deltaTime;
            Vector3 nextPosition = transform.position + direction * step;

            if (Vector3.Distance(nextPosition, pointB.position) < step){
                transform.position = pointB.position;
                break;
            }

            transform.position = nextPosition;
            yield return null;
        }

        transform.position = pointB.position;
    }

    private class TriggerListener : MonoBehaviour{
        public System.Action OnPlayerEnterTrigger;

        private void OnTriggerEnter(Collider other){
            if (other.CompareTag("Player")) OnPlayerEnterTrigger?.Invoke();
        }
    }
}
