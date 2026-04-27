using UnityEngine;

public class DamageAbility : IAbility
{
    [SerializeField] private float attackMultiplier;
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
    }
}