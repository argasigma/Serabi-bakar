using UnityEngine;
using UnityEngine.UI;

public class heartscript : MonoBehaviour
{
    public Image[] hearts;      

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public int maxHealth = 100;
    public int currentHealth = 100;

    public void Start()
    {
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartValue = currentHealth - (i * 20);

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHearts();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHearts();
    }
}