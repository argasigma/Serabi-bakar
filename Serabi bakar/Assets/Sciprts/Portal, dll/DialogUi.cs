using UnityEngine;
using TMPro;
using System.Collections;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance;

    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public float displayDuration = 2f;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        dialogPanel.SetActive(false);
    }

    public bool IsShowingDialog()
    {
        return dialogPanel.activeSelf;
    }

    public void ShowDialog(string message)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(DisplayRoutine(message));
    }

    IEnumerator DisplayRoutine(string message)
    {
        dialogText.text = message;
        dialogPanel.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        dialogPanel.SetActive(false);
    }
}