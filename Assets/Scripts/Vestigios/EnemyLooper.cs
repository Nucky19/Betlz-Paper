using System.Collections;
using UnityEngine;

public class EnemyLooper : MonoBehaviour
{
    public enum MovementAxis { Horizontal, Vertical }

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool startFromA = true;
    [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal;

    private Vector3 direction;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = startFromA ? pointA.position : pointB.position;
        transform.position = startPosition;

        direction = startFromA 
            ? (pointB.position - pointA.position).normalized 
            : (pointA.position - pointB.position).normalized;
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
            transform.position = startPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
