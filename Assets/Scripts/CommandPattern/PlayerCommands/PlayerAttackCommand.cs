using UnityEngine;

public class PlayerAttackCommand : PlayerBaseCommand
{
    // public float attackAnimDuration = 0.2f;
    public AnimationData attackAnimData = new(AnimationData.PriorityLevel.Override, "Attack1", 0.2f);
    public PlayerAttackCommand(IEntity player) : base(player)
    {
    }

    public override void Execute()
    {
        Debug.Log("PlayerAttackCommand executed");
        // player.AnimationController.CrossFade(AnimationController.AttackHash, 0.1f);
    }

    public override async Awaitable ExecuteAsync()
    {
        Debug.Log("PlayerAttackCommand executed");
        await player.AnimationController.PlayAnimation(attackAnimData);
        // player.AnimationController.CrossFade(AnimationController.AttackHash, 0.1f);
        // await Awaitable.WaitForSecondsAsync(attackAnimDuration);
    }
}