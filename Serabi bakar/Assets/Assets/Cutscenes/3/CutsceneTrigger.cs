using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private int cutsceneIndex;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed)
            return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;
            CutsceneManager.Instance.PlayCutscene(cutsceneIndex);
        }
    }
}