using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup secondaryMenu;
    [SerializeField] private float fadeDuration = 1f; // Duración del fade

    [Header("Imagen que cambia de color")]
    [SerializeField] private Image imageToChangeColor;

    [Header("Imagen que cambia de sprite")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite[] sprites; // Lista de imágenes
    [SerializeField] private float imageChangeInterval = 0.5f;
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Referencia al CanvasGroup para el fade
    [SerializeField] private float screenFadeDuration = 1f; // Duración del fade
    private int currentSpriteIndex = 0;

    private Color colorMenu1 = new Color(1f, 1f, 1f); // RGB (255,255,255)
    private Color colorMenu2 = new Color(147f / 255f, 147f / 255f, 147f / 255f); // RGB (147,147,147)

    private void Start()
    {
        InitializeMenus();
        StartCoroutine(SwitchMenusRoutine());
        StartCoroutine(ChangeImageRoutine());
    }

    private void InitializeMenus()
    {
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        secondaryMenu.alpha = 0;
        secondaryMenu.interactable = false;
        secondaryMenu.blocksRaycasts = false;

        if (imageToChangeColor != null)
        {
            imageToChangeColor.color = colorMenu1; // Empieza con el color del menú 1
        }
    }

    private IEnumerator SwitchMenusRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            bool isMainMenuActive = mainMenu.alpha == 1;

            // Fades del menú
            yield return StartCoroutine(FadeCanvasGroup(isMainMenuActive ? mainMenu : secondaryMenu, 1, 0));
            yield return StartCoroutine(FadeCanvasGroup(isMainMenuActive ? secondaryMenu : mainMenu, 0, 1));

            // Cambio de color
            if (imageToChangeColor != null)
            {
                StartCoroutine(FadeImageColor(imageToChangeColor, isMainMenuActive ? colorMenu1 : colorMenu2, isMainMenuActive ? colorMenu2 : colorMenu1));
            }
        }
    }

    private IEnumerator ChangeImageRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(imageChangeInterval);

            if (sprites.Length > 0)
            {
                currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
                targetImage.sprite = sprites[currentSpriteIndex];
            }
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        canvasGroup.interactable = endAlpha == 1;
        canvasGroup.blocksRaycasts = endAlpha == 1;
    }

    private IEnumerator FadeImageColor(Image image, Color startColor, Color endColor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            image.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = endColor;
    }

    public void SceneLoad(string sceneName){
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
   public void OnExitButtonPressed()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    
}
