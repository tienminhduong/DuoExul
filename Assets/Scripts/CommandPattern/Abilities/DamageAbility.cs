using UnityEngine;

public class DamageAbility : IAbility
{
    [SerializeField] private float attackMultiplier;
    public void ApplyEffect(IEntity executor, IEntity target)
    {
        if (target == null)
        {
            Debug.LogWarning("DamageAbility: Target is null. Cannot apply damage.");
            return;
        }
        int damage = (int)(executor.BaseAttack * attackMultiplier);
        target.HealthComponent.TakeDamage(damage);
    }
}