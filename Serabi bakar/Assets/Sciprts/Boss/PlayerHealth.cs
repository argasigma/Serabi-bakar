using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHearts = 5;
    private int currentHearts;

    [Header("UI Hearts")]
    [Tooltip("Drag 5 GameObject heart secara berurutan (index 0 = hati pertama)")]
    public GameObject[] heartIcons;

    [Header("Invincibility (opsional, biar gak kena damage bertubi-tubi)")]
    public float invincibleDuration = 1f;
    private bool isInvincible = false;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
    }

    public void TakeDamage(int amount = 1)
    {
        if (isInvincible) return;

        currentHearts -= amount;
        currentHearts = Mathf.Max(currentHearts, 0);
        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    public void Heal(int amount = 1)
    {
        currentHearts = Mathf.Min(currentHearts + amount, maxHearts);
        UpdateHeartsUI();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null)
                heartIcons[i].SetActive(i < currentHearts);
        }
    }

    System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player mati!");
        // TODO: trigger game over / respawn / animasi mati
    }
}