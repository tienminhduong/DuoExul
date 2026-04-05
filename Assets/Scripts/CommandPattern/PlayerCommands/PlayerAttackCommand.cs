using UnityEngine;

public class PlayerAttackCommand : PlayerBaseCommand
{
    public AnimationData attackAnimData = new(AnimationData.PriorityLevel.Override, "Attack1", 0.2f);
    public PlayerAttackCommand(IEntity player) : base(player)
    {
    }

    public override void Execute()
    {
    }

    public override async Awaitable ExecuteAsync()
    {
        Debug.Log("PlayerAttackCommand executed");
        await player.AnimationController.PlayAnimation(attackAnimData);
    }
}