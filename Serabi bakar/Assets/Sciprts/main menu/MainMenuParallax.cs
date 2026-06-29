using UnityEngine;

public class MainMenuParallax : MonoBehaviour
{
    public float moveDistance = 20f;
    public float smoothSpeed = 2.5f;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Vector2 targetPos = startPosition + new Vector2(mouseX * moveDistance, mouseY * moveDistance);

        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}