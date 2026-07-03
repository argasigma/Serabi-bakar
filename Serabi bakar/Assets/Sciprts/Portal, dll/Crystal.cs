using UnityEngine;

public class Crystal : MonoBehaviour, IInteractable
{
    public string GetPrompt()
    {
        return "Tekan E untuk mengambil kristal";
    }

    public void Interact()
    {
        CrystalManager.Instance.AddCrystal();
        Destroy(gameObject);
    }
}