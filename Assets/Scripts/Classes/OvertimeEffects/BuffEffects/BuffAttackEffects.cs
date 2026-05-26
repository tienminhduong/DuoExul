using UnityEngine;

public class BuffAttackEffects : BaseOvertimeEffect
{
    public float modifier;
    public float maxDuration;

    public BuffAttackEffects()
    {
        EffectKey = "BuffAttack";
        MaxDuration = maxDuration;
    }

    public override bool ApplyEffect(PlayerStat entity)
    {
        if (!base.ApplyEffect(entity))
            return false;

        entity.baseAttack *= modifier;
        return true;
    }

    public override bool RemoveEffect(PlayerStat entity)
    {
        if (!base.RemoveEffect(entity))
            return false;

        entity.baseAttack /= modifier;
        return true;
    }
}