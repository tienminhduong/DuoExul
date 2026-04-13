using UnityEngine;

public class AttackCommand : ICommand
{
    public AttackData attackData;
    public async Awaitable Execute(IEntity entity)
    {
        if (entity is not IAttacker attacker)
        {
            Debug.LogWarning("AttackCommand: Entity does not implement IAttacker. Cannot execute attack.");
            return;
        }
        // Debug.Log("PlayerAttackCommand executed");
        attacker.Attack(attackData);
        await attacker.AnimationController.PlayAnimation(attackData.animationData);
    }
}