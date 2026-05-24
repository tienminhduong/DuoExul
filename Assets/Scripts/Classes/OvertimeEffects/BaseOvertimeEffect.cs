public class BaseOvertimeEffect : IOvertimeEffect
{
    public float MaxDuration { get; protected set; }
    public float CurrentTime { get; protected set; }
    public string EffectKey { get; protected set; }
    public virtual bool ApplyEffect(PlayerStat entity)
    {
        if (entity.OvertimeEffects.Contains(this))
            return false;

        entity.OvertimeEffects.Add(this);
        return true;
    }

    public virtual bool RemoveEffect(PlayerStat entity)
    {
        if (entity.OvertimeEffects.Contains(this))
        {
            entity.OvertimeEffects.Remove(this);
            return true;
        }
        return false;
    }

    public virtual bool UpdateEffect(float deltaTime, PlayerStat entity)
    {
        CurrentTime += deltaTime;
        if (CurrentTime >= MaxDuration)
        {
            RemoveEffect(entity);
        }
        return true;
    }
}