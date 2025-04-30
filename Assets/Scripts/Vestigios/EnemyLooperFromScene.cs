using UnityEngine;

public class EnemyLooperFromScene : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private Vector3 direction;
    private Vector3 startPosition;

    private void Start()
    {
        // Guarda la posición inicial en la escena como spawn original
        startPosition = transform.position;

        // Calcula la dirección de movimiento (de A hacia B)
        direction = (pointB.position - pointA.position).normalized;
    }

    private void OnEnable()
    {
        PlayerStates.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerStates.OnDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath(int screen, bool death)
    {
        if (death)
        {
            transform.position = startPosition;
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            // Teletransporta a punto A
            transform.position = pointA.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
