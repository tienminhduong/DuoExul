using UnityEngine;

public struct ChangeHealthPlayerEvent : IEvent
{
    public float HealthChange { get; private set; }
    public ChangeHealthPlayerEvent(float healthChange)
    {
        HealthChange = healthChange;
    }
}
