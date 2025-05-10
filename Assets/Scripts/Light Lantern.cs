using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private GameObject luz;
    [SerializeField] private float minNormalTime = 3f;
    [SerializeField] private float maxNormalTime = 8f;
    [SerializeField] private float minFlickerTime = 0.05f;
    [SerializeField] private float maxFlickerTime = 0.2f;
    [SerializeField] private int minFlickerCount = 2;
    [SerializeField] private int maxFlickerCount = 5;
    [SerializeField] private bool isFlickering = true;

    void Start()
    {
        if (luz == null) luz = this.gameObject;
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (isFlickering)
        {
            // Mantiene la luz encendida por un tiempo aleatorio
            luz.SetActive(true);
            yield return new WaitForSeconds(Random.Range(minNormalTime, maxNormalTime));

            // Realiza un ciclo de parpadeos
            int flickerCount = Random.Range(minFlickerCount, maxFlickerCount);
            for (int i = 0; i < flickerCount; i++)
            {
                luz.SetActive(false);
                yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

                luz.SetActive(true);
                yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
            }
        }
    }

    public void StopFlickering()
    {
        isFlickering = false;
        luz.SetActive(true); // Asegura que se quede encendida
    }

    public void StartFlickering()
    {
        isFlickering = true;
        StartCoroutine(FlickerRoutine());
    }
}
