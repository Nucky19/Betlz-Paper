using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Animacion_texto : MonoBehaviour
{ public float fadeDuration = 0.5f; // Duración del fade in y fade out
    public float displayDuration = 2.0f; // Tiempo que el mensaje estará en pantalla
    public float initialDelay = 1.0f; // Tiempo de espera antes de mostrar la primera frase
    public float delayBetweenMessages = 1.0f; // Tiempo de espera entre frases
    public string[] messages; // Mensajes a mostrar
    private Text textComponent; // Componente de texto
    private int currentMessageIndex = 0;
    [SerializeField] bool Final=false;

    void Start()
    {
        textComponent = GetComponent<Text>();
        StartCoroutine(ShowMessages());
    }

    void Update(){
        if(Input.GetKeyDown("p")){
            if(!Final) SceneManager.LoadScene("Level1");
            else SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator ShowMessages()
    {
        // Espera inicial antes de mostrar la primera frase
        yield return new WaitForSeconds(initialDelay);

        while (currentMessageIndex < messages.Length)
        {
            yield return StartCoroutine(ShowMessage(messages[currentMessageIndex]));
            currentMessageIndex++;

            // Espera entre mensajes
            if (currentMessageIndex < messages.Length)
            {
                yield return new WaitForSeconds(delayBetweenMessages);
            }
        }
        if(!Final) SceneManager.LoadScene("Level1");
        else SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ShowMessage(string message)
    {
        textComponent.text = message; // Establece el texto completo
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0); // Comienza con opacidad 0

        // Fade in
        yield return StartCoroutine(FadeTextTo(1, fadeDuration)); // Fade in a opacidad 1

        // Espera el tiempo que el mensaje debe estar en pantalla
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return StartCoroutine(FadeTextTo(0, fadeDuration)); // Fade out a opacidad 0
    }

    private IEnumerator FadeTextTo(float aValue, float aTime)
    {
        float alpha = textComponent.color.a; // Obtiene el valor actual de alpha
        float rate = 1.0f / aTime; // Calcula la tasa de cambio
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress += rate * Time.deltaTime; // Incrementa el progreso
            Color newColor = textComponent.color; // Crea un nuevo color
            newColor.a = Mathf.Lerp(alpha, aValue, progress); // Interpola el valor de alpha
            textComponent.color = newColor; // Aplica el nuevo color
            yield return null; // Espera un frame
        }
    }
}
