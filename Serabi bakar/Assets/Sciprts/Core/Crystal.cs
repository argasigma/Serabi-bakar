using UnityEngine;

public class Crystal : MonoBehaviour
{
    public int amount = 1;
    public GameObject interactUI;

    private bool canInteract = false;

    void Start()
    {
        if (interactUI != null)
            interactUI.SetActive(false);
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            CrystalManager.instance.AddCrystal(amount);

            if (interactUI != null)
                interactUI.SetActive(false);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;

            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;

            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}