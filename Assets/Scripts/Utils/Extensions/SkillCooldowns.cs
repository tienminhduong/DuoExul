using System.Collections.Generic;
using UnityEngine;

/// <summary>Quản lý cooldown tất cả skill của một boss.</summary>
public class SkillCooldowns
{
    readonly Dictionary<string, float> _remaining = new();
    readonly Dictionary<string, float> _base = new();

    public void Register(string id, float cd)
    {
        _base[id] = cd;
        _remaining[id] = 0f;   // sẵn sàng ngay lúc đầu
    }

    /// <summary>Gọi mỗi Update.</summary>
    public void Tick(float dt)
    {
        foreach (var key in new List<string>(_remaining.Keys))
            _remaining[key] = Mathf.Max(0f, _remaining[key] - dt);
    }

    public bool IsReady(string id) => _remaining.TryGetValue(id, out var t) && t <= 0f;

    /// <summary>Đặt lại cooldown sau khi dùng skill.</summary>
    public void Use(string id) { if (_base.ContainsKey(id)) _remaining[id] = _base[id]; }

    /// <summary>Phase 2: rút ngắn tất cả cooldown.</summary>
    public void ScaleAll(float mult)
    {
        foreach (var key in new List<string>(_base.Keys))
            _base[key] *= mult;
    }
}
