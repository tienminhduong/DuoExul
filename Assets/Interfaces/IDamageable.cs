using UnityEngine;

public interface IDamageable
{
    bool IsInvincible { get; }
    void TakeDamage(int amount, Vector2 knockback);
    void Die();
}