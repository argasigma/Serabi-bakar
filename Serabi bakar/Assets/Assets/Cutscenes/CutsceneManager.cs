using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    public GameObject cutscenePanel;
    public Image image; // cutscene image game object

    public Sprite[] cutsceneSprites;
    private int currentI; // current index

    public CanvasGroup canvasGroup;
    public float fadeSpeed = 0.5f;
    private bool isTransitioning = false;

    public CanvasGroup blackCanvasGroup;
    public GameObject blackScreen;

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
        
        StartCoroutine(ChangeImage());
    }

    public void EndCutscene()
    {
        StartCoroutine(EndCutsceneRoutine());
    }

    IEnumerator EndCutsceneRoutine()
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1.4f);

        cutscenePanel.SetActive(false);

        yield return StartCoroutine(BlackScreenFadeOut());

        GameManager.Instance.currentState = GameState.Playing;
        isTransitioning = false;
    }

    IEnumerator BlackScreenFadeOut()
    {
        while (blackCanvasGroup.alpha > 0)
        {
            blackCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        blackCanvasGroup.alpha = 0;
        blackScreen.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    // alur/flow fade in setiap berubah cutscene
    IEnumerator ChangeImage()
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1.4f);

        image.sprite = cutsceneSprites[currentI];

        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }
}