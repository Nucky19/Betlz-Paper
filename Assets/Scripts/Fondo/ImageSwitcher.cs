using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Referencias")]
    public Image targetImage;             // Imagen que se va a animar
    public Sprite[] animationFrames;      // Lista de imágenes (frames de animación)
    public float[] frameDurations;        // Duración individual de cada frame

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

        if (frameDurations == null || frameDurations.Length != animationFrames.Length)
        {
            Debug.LogWarning("Duraciones no asignadas correctamente. Se asignará duración por defecto (0.2s) a todos los frames.");
            frameDurations = new float[animationFrames.Length];
            for (int i = 0; i < frameDurations.Length; i++)
                frameDurations[i] = 0.2f;
        }

        targetImage.sprite = animationFrames[0];
        timer = frameDurations[0];
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            targetImage.sprite = animationFrames[currentFrame];
            timer = frameDurations[currentFrame];
        }
    }
}