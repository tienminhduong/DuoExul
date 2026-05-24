using UnityEngine.Events;

public interface IOvertimeEffect
{
    bool ApplyEffect(PlayerStat entity);

    bool RemoveEffect(PlayerStat entity);
    bool UpdateEffect(float deltaTime, PlayerStat entity);
}