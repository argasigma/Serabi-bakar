using UnityEngine;

public class Crystal : MonoBehaviour
{
    private bool taken = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (taken) return;
        if (!other.CompareTag("Player")) return;

        taken = true;

        Destroy(gameObject);

        CutsceneManager.Instance.PlayCutscene(2);
    }
}