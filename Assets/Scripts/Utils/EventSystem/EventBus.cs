using System;
using System.Collections.Generic;

public static class EventBus<T> where T : IEvent
{
    public static Action<T> OnEvent = _ => { };
    public static Action OnEventNoParam = () => { };

    public static void RaiseEvent(T eventData)
    {
        OnEvent(eventData);
        OnEventNoParam();
    }

    public static void Register(Action<T> onEvent) => OnEvent += onEvent;
    public static void Register(Action onEventNoParam) => OnEventNoParam += onEventNoParam;

    public static void Unregister(Action<T> onEvent) => OnEvent -= onEvent;
    public static void Unregister(Action onEventNoParam) => OnEventNoParam -= onEventNoParam;
}