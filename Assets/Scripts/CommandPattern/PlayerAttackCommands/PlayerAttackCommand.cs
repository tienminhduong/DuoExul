using UnityEngine;

public class PlayerAttackCommand : ICommand
{
    public AttackData attackData;
    public async Awaitable Execute(IEntity entity)
    {
        // Debug.Log("PlayerAttackCommand executed");
        entity.Attack(attackData);
        await entity.AnimationController.PlayAnimation(attackData.animationData);
    }
}