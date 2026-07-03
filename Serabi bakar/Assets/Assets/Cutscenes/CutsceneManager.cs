using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    public GameObject cutscenePanel;
    public Image image; // cutscene image game object

    public Sprite[] cutsceneSprites;
    [SerializeField] private bool[] useFade;
    private int currentI; // current index

    public CanvasGroup canvasGroup;
    private float imageFadeSpeed = 0.7f;
    private bool isTransitioning = false;

    public CanvasGroup blackCanvasGroup;
    public GameObject blackScreen;

    // texts
    public TMP_Text cutsceneText;
    private float textFadeSpeed = 3f;
    public CanvasGroup textCanvasGroup;
    private Coroutine blinkCoroutine;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.currentState = GameState.Cutscene;
        StartCutscene();
    }

    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Cutscene)
            return;

        if (isTransitioning)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextImage();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EndCutscene();
        }
    }

    void NextImage()
    {
        currentI++;

        if (currentI >= cutsceneSprites.Length)
        {
            EndCutscene();
            return;
        }

        StartCoroutine(ChangeImage());
    }

    public void StartCutscene()
    {
        GameManager.Instance.currentState = GameState.Cutscene;
        cutscenePanel.SetActive(true);
        blackScreen.SetActive(true);

        currentI = 0;
        canvasGroup.alpha = 0;
        textCanvasGroup.alpha = 0;
        
        StartCoroutine(ChangeImage());
    }

    public void EndCutscene()
    {
        if (isTransitioning)
            return;

        StartCoroutine(EndCutsceneRoutine());
    }

    IEnumerator EndCutsceneRoutine()
    {
        isTransitioning = true;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        yield return StartCoroutine(TextFadeOut());
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(3.5f);

        cutscenePanel.SetActive(false);

        yield return StartCoroutine(BlackScreenFadeOut());

        GameManager.Instance.currentState = GameState.Playing;
        isTransitioning = false;
    }

    IEnumerator BlackScreenFadeOut()
    {
        while (blackCanvasGroup.alpha > 0)
        {
            blackCanvasGroup.alpha -= Time.deltaTime * imageFadeSpeed;
            yield return null;
        }

        blackCanvasGroup.alpha = 0;
        blackScreen.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * imageFadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * imageFadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    IEnumerator TextFadeOut()
    {
        while (textCanvasGroup.alpha > 0)
        {
            textCanvasGroup.alpha -= Time.deltaTime * textFadeSpeed;
            yield return null;
        }

        textCanvasGroup.alpha = 0;
    }

    IEnumerator TextFadeIn()
    {
        while (textCanvasGroup.alpha < 1)
        {
            textCanvasGroup.alpha += Time.deltaTime * textFadeSpeed;
            yield return null;
        }

        textCanvasGroup.alpha = 1;
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            // Fade out
            while (textCanvasGroup.alpha > 0)
            {
                textCanvasGroup.alpha -= Time.deltaTime * 1.5f;
                yield return null;
            }

            textCanvasGroup.alpha = 0;

            yield return new WaitForSeconds(0.3f);

            // Fade in
            while (textCanvasGroup.alpha < 1)
            {
                textCanvasGroup.alpha += Time.deltaTime * 1.5f;
                yield return null;
            }

            textCanvasGroup.alpha = 1;

            yield return new WaitForSeconds(1);
        }
    }

    // alur/flow fade in dan fade out cutscene
    IEnumerator ChangeImage()
    {
        isTransitioning = true;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        bool shouldFade = currentI < useFade.Length && useFade[currentI];

        if (shouldFade)
        {
            yield return StartCoroutine(TextFadeOut());
            yield return StartCoroutine(FadeOut());
            yield return new WaitForSeconds(2f);

            image.sprite = cutsceneSprites[currentI];

            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(TextFadeIn());
            blinkCoroutine = StartCoroutine(BlinkText());
        }
        else
        {
            image.sprite = cutsceneSprites[currentI];
            canvasGroup.alpha = 1;
            textCanvasGroup.alpha = 1;

            blinkCoroutine = StartCoroutine(BlinkText());
        }

        isTransitioning = false;
    }
}