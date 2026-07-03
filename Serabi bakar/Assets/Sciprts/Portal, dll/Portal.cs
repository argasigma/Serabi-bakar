using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PortalInteract : MonoBehaviour, IInteractable
{
    [Header("Tujuan Teleport")]
    public Transform destination;

    [Header("Referensi")]
    public ScreenFader screenFader;
    private Animator animator;

    [Header("State")]
    public bool isOpen = false; // FIX: portal tertutup dulu, baru kebuka setelah boss dikalahkan

    private bool isTransitioning = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (screenFader == null)
            screenFader = FindFirstObjectByType<ScreenFader>(); // ganti dari FindObjectOfType
    }

    // FIX: dipanggil dari Boss.cs saat boss mati
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("Open"); // pastikan Animator Controller portal punya trigger param "Open"

        Debug.Log("[PortalInteract] Portal terbuka!");
    }

    public string GetPrompt()
    {
        if (!isOpen) return ""; // FIX: belum bisa dipakai sebelum boss dikalahkan
        if (isTransitioning) return "";
        return "Tekan E untuk masuk Portal";
    }

    public void Interact()
    {
        if (!isOpen) return; // FIX: blokir interact sebelum portal terbuka
        if (isTransitioning) return;
        EnterPortal();
    }

    private void EnterPortal()
    {
        if (destination == null)
        {
            Debug.LogWarning($"[PortalInteract] '{name}' belum punya destination.");
            return;
        }

        isTransitioning = true;

        if (screenFader != null)
        {
            screenFader.FadeOutThenIn(TeleportPlayer);
        }
        else
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer() // <- dikembalikan tanpa parameter
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerObj.transform.position = destination.position;
        }

        isTransitioning = false;
    }
}