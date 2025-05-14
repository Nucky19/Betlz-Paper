using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias")]
    public Image targetImage;             // Imagen que se va a animar
    public Sprite[] animationFrames;      // Lista de imágenes (frames de animación)

    [Header("Configuración")]
    public float frameInterval = 0.2f;    // Tiempo entre cada frame (en segundos)

    private int currentFrame = 0;
    private float timer;

    void Start()
    {
        if (targetImage == null || animationFrames == null || animationFrames.Length < 2)
        {
            Debug.LogError("Debes asignar al menos dos imágenes y la referencia del Image.");
            enabled = false;
            return;
        }

        targetImage.sprite = animationFrames[0];
        timer = frameInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            targetImage.sprite = animationFrames[currentFrame];
            timer = frameInterval;
        }
    }
}