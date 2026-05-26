
using UnityEngine;

public class SelfDamageAbility : IAbility
{
    [Tooltip("Percentage of current HP to lose when this ability is used, 0.1 means 10%, 0 if you want to use a fixed amount.")]
    [SerializeField] private float hpLossPercentage;
    [Tooltip("Fixed amount of HP to lose when this ability is used, ignored if hpLossPercentage is greater than 0.")]
    [SerializeField] private int fixedHpLossAmount;
    public void ApplyEffect(IEntity executor, IEntity target)
    {
        if (executor is IDamageable damageableExecutor)
        {
            if (hpLossPercentage > 0)
            {
                int damageAmount = Mathf.CeilToInt(damageableExecutor.HealthComponent.CurrentHealth * hpLossPercentage);
                damageableExecutor.HealthComponent.TakeDamage(damageAmount);
            }
            else
            {
                damageableExecutor.HealthComponent.TakeDamage(fixedHpLossAmount);
            }
        }
    }
}