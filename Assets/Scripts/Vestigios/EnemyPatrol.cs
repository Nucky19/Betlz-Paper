using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform target;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Coroutine patrolRoutine;

    void Start(){
        target = pointB;
        startPosition = pointA.position;
        startRotation = transform.rotation;

        transform.position = startPosition;
        patrolRoutine = StartCoroutine(PatrolRoutine());
    }

    private void OnEnable(){
        PlayerStates.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable(){
        PlayerStates.OnDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(int screen, bool death){
        if (death){
            if (patrolRoutine != null) StopCoroutine(patrolRoutine);

            transform.position = startPosition;
            transform.rotation = startRotation;

            target = pointB;
            patrolRoutine = StartCoroutine(PatrolRoutine());
        }
    }

    IEnumerator PatrolRoutine(){
        while (true){
            while (Vector3.Distance(transform.position, target.position) > 0.1f){
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            target = target == pointA ? pointB : pointA;
        }
    }
}
