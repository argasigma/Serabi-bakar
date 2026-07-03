using UnityEngine;

public class LastPortal : MonoBehaviour, IInteractable
{
    [Header("State")]
    public bool isUnlocked = false;
    private bool isBusy = false; // true selama animasi unlock berjalan

    [Header("Animasi")]
    public Animator portalAnimator;
    public string unlockTriggerName = "Unlock";

    [Header("Efek Tambahan (opsional)")]
    public ParticleSystem unlockParticle;
    public AudioSource audioSource;
    public AudioClip unlockSfx;

    void Start()
    {
        if (portalAnimator != null)
            portalAnimator.ResetTrigger(unlockTriggerName);
    }

    public string GetPrompt()
    {
        if (isBusy) return ""; // gak usah kasih prompt selama animasi jalan
        return isUnlocked ? "Tekan E untuk masuk Portal" : "Tekan E untuk buka Portal";
    }

    public void Interact()
    {
        if (isBusy) return; // cegah spam interact selama animasi

        if (isUnlocked)
        {
            Debug.Log("Player masuk portal!");
            return;
        }

        if (CrystalManager.Instance.HasEnoughCrystals())
        {
            isBusy = true;
            isUnlocked = true;
            PlayUnlockAnimation();
            DialogUI.Instance.ShowDialog("Portalnya terbuka!");
            StartCoroutine(EndBusyAfterDelay());
        }
        else
        {
            DialogUI.Instance.ShowDialog("Portal terkunci, aku harus mencari 3 kristal untuk membuka portal ini.");
        }
    }

    System.Collections.IEnumerator EndBusyAfterDelay()
    {
        if (portalAnimator != null)
        {
            yield return null; // tunggu 1 frame biar Animator update state
            AnimatorStateInfo state = portalAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(state.length);
        }
        isBusy = false;
    }

    void PlayUnlockAnimation()
    {
        if (portalAnimator != null)
            portalAnimator.SetTrigger(unlockTriggerName);

        if (unlockParticle != null)
            unlockParticle.Play();

        if (audioSource != null && unlockSfx != null)
            audioSource.PlayOneShot(unlockSfx);
    }
}