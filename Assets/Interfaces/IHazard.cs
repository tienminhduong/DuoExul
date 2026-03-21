using UnityEngine;

public interface IHazard
{
    int Damage { get; }
    Vector2 KnockbackForce { get; }
    bool IsActive { get; }
    HitSourceType SourceType { get; }

}
