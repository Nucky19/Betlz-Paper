using UnityEngine;

public class EnemyLooper : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private Vector3 direction;

    private void Start() {
        direction = (pointB.position - pointA.position).normalized;
        transform.position = pointA.position;
    }

    private void OnEnable() {
        PlayerStates.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable(){
        PlayerStates.OnDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(int screen, bool death){
        if (death) transform.position = pointA.position;
    }

    private void Update(){
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("EndPoint")) transform.position = pointA.position;
    }
}