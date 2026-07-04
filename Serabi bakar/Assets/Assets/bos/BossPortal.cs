using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossPortal : MonoBehaviour
{
    public Transform destination;
    public ScreenFader screenFader;

    private Animator animator;
    public bool isOpen = false;

    private bool isTransitioning = false;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (screenFader == null)
            screenFader = FindFirstObjectByType<ScreenFader>();
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;

        if (animator != null)
            animator.SetTrigger("Open");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen || isTransitioning)
            return;

        if (!other.CompareTag("Player"))
            return;

        isTransitioning = true;

        screenFader.FadeOutThenIn(() =>
        {
            other.transform.position = destination.position;
            isTransitioning = false;
        });
    }
}