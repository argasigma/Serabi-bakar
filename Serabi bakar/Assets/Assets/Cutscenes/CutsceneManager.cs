using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    public GameObject cutscenePanel;
    public Image image; // cutscene image game object

    [SerializeField] private CutsceneData[] cutscenes;
    private int currentCutscene = 0;

    private Sprite[] cutsceneSprites;
    private bool[] useFade;

    private int currentI; // current index

    public CanvasGroup canvasGroup;
    private float imageFadeSpeed = 0.7f;
    private bool isTransitioning = false;

    public CanvasGroup blackCanvasGroup;
    public GameObject blackScreen;
    
    public GameObject firePortal; // Variabel untuk portal

    // texts
    public TMP_Text cutsceneText;
    private float textFadeSpeed = 1.5f;
    public CanvasGroup textCanvasGroup;
    private Coroutine blinkCoroutine;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayCutscene(0);
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

    public void PlayCutscene(int index)
    {
        currentCutscene = index;

        cutsceneSprites = cutscenes[index].sprites;
        useFade = cutscenes[index].useFade;

        StartCutscene();
    }

    public void StartCutscene()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
        {
            player.StopPlayer();
        }

        GameManager.Instance.currentState = GameState.Cutscene;

        cutscenePanel.SetActive(true);
        blackScreen.SetActive(true);
        blackCanvasGroup.alpha = 1;

        currentI = 0;
        canvasGroup.alpha = 0;
        textCanvasGroup.alpha = 0;

        StartCoroutine(ChangeImage());

        if (useFade.Length == 0 || !useFade[0])
        {
            StartCoroutine(StartText());
        }
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

        if (currentCutscene == 2)
        {
            SceneManager.LoadScene("MainMenu");
            yield break;
        }

        cutscenePanel.SetActive(false);

        yield return StartCoroutine(BlackScreenFadeOut());

        // Memastikan portal hanya menyala jika cutscene yang selesai adalah index 1
        // (Ubah angka 1 jika index cutscene rumah Anda berbeda)
        if (firePortal != null && currentCutscene == 1)
        {
            firePortal.SetActive(true);
        }

        GameManager.Instance.currentState = GameState.Playing;
        isTransitioning = false;
    }

    IEnumerator StartText()
    {
        while (isTransitioning)
            yield return null;

        yield return new WaitForSeconds(2f);

        if (isTransitioning)
            yield break;

        yield return StartCoroutine(TextFadeIn());

        if (blinkCoroutine == null)
            blinkCoroutine = StartCoroutine(BlinkText());
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
            yield return new WaitForSeconds(1f);

            while (textCanvasGroup.alpha > 0)
            {
                textCanvasGroup.alpha -= Time.deltaTime * textFadeSpeed;
                yield return null;
            }

            textCanvasGroup.alpha = 0;

            yield return new WaitForSeconds(0.3f);

            while (textCanvasGroup.alpha < 1)
            {
                textCanvasGroup.alpha += Time.deltaTime * textFadeSpeed;
                yield return null;
            }

            textCanvasGroup.alpha = 1;
        }
    }

    // alur/flow fade in dan fade out cutscene
    IEnumerator ChangeImage()
    {
        isTransitioning = true;

        bool shouldFade = currentI < useFade.Length && useFade[currentI];

        if (shouldFade)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            yield return StartCoroutine(TextFadeOut());
            yield return StartCoroutine(FadeOut());
            yield return new WaitForSeconds(2f);

            image.sprite = cutsceneSprites[currentI];

            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(TextFadeIn());

            blinkCoroutine = StartCoroutine(BlinkText());
        }

        else
        {
            image.sprite = cutsceneSprites[currentI];
            canvasGroup.alpha = 1;
        }

        isTransitioning = false;
    }
}