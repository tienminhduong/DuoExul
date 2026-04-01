using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDTAbilityData", menuName = "Scriptable Objects/DDTAbilityData")]
public class DDTAbilityData : ScriptableObject
{
    public string label;
    [SerializeReference] public List<DDTAbilityEffect> effects;

    void OnEnable()
    {
        if (string.IsNullOrEmpty(label))
            label = name;

        if (effects == null)
            effects = new List<DDTAbilityEffect>();
    }
}

[Serializable]
public abstract class DDTAbilityEffect
{
    public abstract void Execute(GameObject caster, GameObject target);
}

[Serializable]
public class DDTDamageEffect : DDTAbilityEffect
{
    public int damageAmount;

    public override void Execute(GameObject caster, GameObject target)
    {
        target.GetComponent<DDTHealth>()?.TakeDamage(damageAmount);
    }
}


[Serializable]
public class DDTKnockbackEffect : DDTAbilityEffect
{
    public float knockbackForce;

    public override void Execute(GameObject caster, GameObject target)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (target.transform.position - caster.transform.position).normalized;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }
}