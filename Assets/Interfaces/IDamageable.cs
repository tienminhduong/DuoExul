using UnityEngine;

public interface IDamageable : IEntity
{
    HealthComponent HealthComponent { get; }
}