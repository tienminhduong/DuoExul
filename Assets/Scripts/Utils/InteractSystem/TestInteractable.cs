using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public Vector2 Position => transform.position;
    public void OnInteract()
    {
        Debug.Log("Interacted with TestInteractable at position: " + Position);
        Destroy(gameObject);
    }

    void OnEnable() => EventBus<OverlappedEvent>.Register(SayHaha);

    void OnDisable() => EventBus<OverlappedEvent>.Unregister(SayHaha);

    private void SayHaha(OverlappedEvent evt)
    {
        Debug.Log("Haha! from " + gameObject.name);
    }
}