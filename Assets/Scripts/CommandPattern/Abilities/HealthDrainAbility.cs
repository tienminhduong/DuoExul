using UnityEngine;

public class HealthDrainAbility : IAbility
{
    [SerializeField] private float drainPercentage; // Percentage of target's current HP to drain
    [SerializeField] private float attackMultiplier; // Multiplier for calculating damage to self based on target's HP drained
    public void ApplyEffect(IEntity executor, IEntity target)
    {
        if (executor is not IAttacker attacker)
        {
            Debug.LogWarning("DamageAbility: Executor does not implement IAttacker. Cannot apply damage.");
            return;
        }
        int damage = (int)(attacker.BaseAttack * attackMultiplier);
        if (target == null || target is not IDamageable damageableTarget)
        {
            Debug.LogWarning("DamageAbility: Target is not damageable. Cannot apply damage.");
            return;
        }
        damageableTarget.HealthComponent.TakeDamage(damage);
        int hpDrained = Mathf.CeilToInt(damageableTarget.HealthComponent.CurrentHealth * drainPercentage);
        if (executor is IDamageable damageableExecutor)
        {
            damageableExecutor.HealthComponent.Heal(hpDrained);
        }
    }
}