using UnityEngine;
using UnityEngine.UI;

public class IconSkillManager : MonoBehaviour
{
    public static IconSkillManager Instance;

    [Header("Skill Icons")]
    [SerializeField] private Image[] skillIcons;

    [Header("Appearance")]
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color normalColor = Color.gray;

    [Header("Animation")]
    [SerializeField] private float selectedOffsetY = 15f;
    [SerializeField] private float moveSpeed = 10f;

    private int selectedSkill = -1;
    private Vector2[] originalPositions;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalPositions = new Vector2[skillIcons.Length];

        for (int i = 0; i < skillIcons.Length; i++)
        {
            originalPositions[i] = skillIcons[i].rectTransform.anchoredPosition;
        }
    }

    void Update()
    {
        UpdateUI();
    }

    public void SelectSkill(int index)
    {
        selectedSkill = index;
    }

    void UpdateUI()
    {
        for (int i = 0; i < skillIcons.Length; i++)
        {
            bool selected = i == selectedSkill;

            skillIcons[i].color = selected ? selectedColor : normalColor;

            Vector2 targetPos = originalPositions[i];

            if (selected)
                targetPos += Vector2.up * selectedOffsetY;

            RectTransform rect = skillIcons[i].rectTransform;

            rect.anchoredPosition = Vector2.Lerp(
                rect.anchoredPosition,
                targetPos,
                Time.deltaTime * moveSpeed);

            Vector3 targetScale = selected ? Vector3.one * 1.15f : Vector3.one;

            rect.localScale = Vector3.Lerp(
                rect.localScale,
                targetScale,
                Time.deltaTime * moveSpeed);
        }
    }
}
