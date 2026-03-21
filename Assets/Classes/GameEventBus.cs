using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventBus
{
    // Dictionary<Type, Delegate> để map event type → handlers
    private static readonly Dictionary<Type, Delegate> _handlers = new();

    /// <summary>
    /// Subscribe một handler cho event type T.
    /// Gọi trong OnEnable, hủy trong OnDisable để tránh memory leak.
    /// </summary>
    public static void Subscribe<T>(Action<T> handler) where T : struct
    {
        var type = typeof(T);
        if (_handlers.TryGetValue(type, out var existing))
            _handlers[type] = Delegate.Combine(existing, handler);
        else
            _handlers[type] = handler;
    }

    /// <summary>
    /// Unsubscribe handler. LUÔN gọi trong OnDisable/OnDestroy.
    /// </summary>
    public static void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        var type = typeof(T);
        if (_handlers.TryGetValue(type, out var existing))
        {
            var updated = Delegate.Remove(existing, handler);
            if (updated == null) _handlers.Remove(type);
            else _handlers[type] = updated;
        }
    }

    /// <summary>
    /// Publish một event. Tất cả subscriber nhận được ngay lập tức (synchronous).
    /// </summary>
    public static void Publish<T>(T evt) where T : struct
    {
        if (_handlers.TryGetValue(typeof(T), out var handler))
            ((Action<T>)handler).Invoke(evt);
    }

    /// <summary>Xóa toàn bộ subscription (dùng khi load scene mới).</summary>
    public static void Clear() => _handlers.Clear();

}
