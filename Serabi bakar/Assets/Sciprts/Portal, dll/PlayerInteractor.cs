using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 1.5f;
    public LayerMask interactableLayer;
    public InteractionUI interactionUI;

    private IInteractable currentTarget;

    void Update()
{
    FindClosestInteractable();

    bool dialogShowing = DialogUI.Instance != null && DialogUI.Instance.IsShowingDialog();

    if (currentTarget != null && !dialogShowing)
    {
        interactionUI.Show(currentTarget.GetPrompt());

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact();
        }
    }
    else
    {
        interactionUI.Hide();
    }
}

    void FindClosestInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, interactableLayer);

        float closestDist = Mathf.Infinity;
        IInteractable closest = null;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = interactable;
                }
            }
        }

        currentTarget = closest;
    }

    // biar keliatan radiusnya di Scene view pas testing
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}