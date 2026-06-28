using UnityEngine;
using UnityEngine.UI;

public class heartscript : MonoBehaviour
{
    public Image[] hearts;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [SerializeField] private PlayerData playerData;
    public float currentHealth;

    public void Start()
    {
        currentHealth = playerData.maxHP;
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            float heartValue = currentHealth - (i * 20);

            if (heartValue >= 20)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (heartValue >= 10)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
        Debug.Log("Current Health : " + currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, playerData.maxHP);

        UpdateHearts();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, playerData.maxHP);

        UpdateHearts();
    }
}