using System.Collections;
using UnityEngine;

public class Cuchilla : MonoBehaviour
{
    public enum PatrolAxis { Vertical, Horizontal }

    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool startFromPointB = false;
    [SerializeField] private PatrolAxis patrolAxis = PatrolAxis.Vertical;

    [Header("Reset Trigger")]
    [SerializeField] private GameObject resetTriggerObject;

    private Transform target;
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
        target = startFromPointB ? pointA : pointB;
        patrolRoutine = StartCoroutine(MoveToTargetOnce());
    }

    private void StartMovement(){
        if (startFromPointB){
            startPosition = pointB.position;
            target = pointA;
        }
        else{
            startPosition = pointA.position;
            target = pointB;
        }

        startRotation = transform.rotation;
        transform.position = startPosition;

        patrolRoutine = StartCoroutine(MoveToTargetOnce());
    }

    IEnumerator MoveToTargetOnce(){
        while (Vector3.Distance(transform.position, target.position) > 0.05f){
            Vector3 direction = (target.position - transform.position).normalized;
            float step = speed * Time.deltaTime;
            Vector3 nextPosition = transform.position + direction * step;

            if (Vector3.Distance(nextPosition, target.position) < step){
                transform.position = target.position;
                break;
            }

            transform.position = nextPosition;

            Vector3 lookDir = direction;
            if (patrolAxis == PatrolAxis.Horizontal)
                lookDir = new Vector3(direction.x, 0, 0);
            else if (patrolAxis == PatrolAxis.Vertical)
                lookDir = new Vector3(0, direction.y, 0);

            if (lookDir != Vector3.zero){
                Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            yield return null;
        }

        transform.position = target.position;
    }

   
    private class TriggerListener : MonoBehaviour{
        public System.Action OnPlayerEnterTrigger;

        private void OnTriggerEnter(Collider other){
            if (other.CompareTag("Player")) OnPlayerEnterTrigger?.Invoke();
        }
    }
}
