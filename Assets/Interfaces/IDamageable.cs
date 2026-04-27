using UnityEngine;

public interface IDamageable : IEntity
{
    // bool IsInvincible { get; }
    HealthComponent HealthComponent { get; }
    // void TakeDamage(int amount, Vector2 knockback);
    // void Die();
}