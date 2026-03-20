using TriInspector;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Interactor : MonoBehaviour
{
    private Collider2D interactRange;

    protected IInteractable overlappedInteractable;

    private void Awake()
    {
        interactRange = GetComponent<Collider2D>();
        interactRange.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent(out IInteractable interactableComponent);
        if (interactableComponent != null)
        {
            overlappedInteractable = interactableComponent;
            EventBus<OverlappedEvent>.RaiseEvent(new OverlappedEvent(this, interactableComponent));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.root.TryGetComponent(out IInteractable interactableComponent);
        if (interactableComponent != null)
        {
            if (overlappedInteractable == interactableComponent)
                overlappedInteractable = null;
            EventBus<UnoverlappedEvent>.RaiseEvent(new UnoverlappedEvent(this, interactableComponent));
        }
    }

    public virtual void HandleInteraction()
    {
        overlappedInteractable?.OnInteract();
    }
}