public class OverlappedEvent : IEvent
{
    public readonly IInteractable Interactable;
    public readonly Interactor Interactor;

    public OverlappedEvent(Interactor interactor, IInteractable interactable)
    {
        Interactor = interactor;
        Interactable = interactable;
    }
}

public class UnoverlappedEvent : IEvent
{
    public readonly IInteractable Interactable;
    public readonly Interactor Interactor;

    public UnoverlappedEvent(Interactor interactor, IInteractable interactable)
    {
        Interactor = interactor;
        Interactable = interactable;
    }
}