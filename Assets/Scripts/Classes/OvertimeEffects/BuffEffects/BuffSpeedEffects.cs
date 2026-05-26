using UnityEngine.Events;

public class BuffSpeedEffects : BaseOvertimeEffect
{
    public float modifier;
    public float maxDuration;

    public BuffSpeedEffects()
    {
        EffectKey = "BuffSpeed";
        MaxDuration = maxDuration;
    }


    public override bool ApplyEffect(PlayerStat entity)
    {
        if (!base.ApplyEffect(entity))
            return false;

        entity.moveSpeed *= modifier;
        return true;
    }

    public override bool RemoveEffect(PlayerStat entity)
    {
        if (!base.RemoveEffect(entity))
            return false;

        entity.moveSpeed /= modifier;
        return true;
    }
}