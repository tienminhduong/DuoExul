
using UnityEngine;

public interface IInteractable
{
    public Vector2 Position { get; }
    void OnInteract() { }
}