using UnityEngine;

/// <summary>
/// Taruh script ini di GameObject Portal (2D).
/// Portal butuh Collider2D dengan "Is Trigger" dicentang.
/// Player butuh tag "Player" dan Rigidbody2D (atau minimal Collider2D).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PortalInteract : MonoBehaviour
{
    [Header("Tujuan Teleport")]
    [Tooltip("Transform tempat player akan muncul setelah masuk portal")]
    public Transform destination;

    [Header("UI Prompt (opsional, bisa auto-cari lewat tag)")]
    [Tooltip("GameObject UI 'Tekan E untuk masuk' yang muncul saat player dekat")]
    public GameObject interactPromptUI;

    [Header("Referensi")]
    public ScreenFader screenFader;

    private bool playerInRange = false;
    private Transform playerTransform;
    private bool isTransitioning = false;

    private void Reset()
    {
        // Pastikan collider ini trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);

        // Auto-cari ScreenFader di scene kalau belum di-assign
        if (screenFader == null)
            screenFader = FindObjectOfType<ScreenFader>();
    }

    private void Update()
    {
        if (playerInRange && !isTransitioning && Input.GetKeyDown(KeyCode.E))
        {
            EnterPortal();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        playerTransform = other.transform;

        if (interactPromptUI != null)
            interactPromptUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);
    }

    private void EnterPortal()
    {
        if (destination == null)
        {
            Debug.LogWarning($"[PortalInteract] '{name}' belum punya destination.");
            return;
        }

        isTransitioning = true;

        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);

        if (screenFader != null)
        {
            // Fade out -> pindahkan player -> fade in
            screenFader.FadeOutThenIn(TeleportPlayer);
        }
        else
        {
            // Kalau tidak ada fader, langsung pindah saja
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (playerTransform != null)
        {
            playerTransform.position = destination.position;
        }

        isTransitioning = false;
    }
}