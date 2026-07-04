using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public GameObject promptPanel;
    public TextMeshProUGUI promptText;

    void Start() => Hide();

    public void Show(string text)
    {
        promptPanel.SetActive(true);
        promptText.text = text;
    }

    public void Hide()
    {
        promptPanel.SetActive(false);
    }
}