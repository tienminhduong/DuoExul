using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttackCommand : PlayerBaseCommand
{
    public float attackAnimDuration = 0.267f;
    public PlayerAttackCommand(IEntity player) : base(player)
    {
    }

    public override void Execute()
    {
        // await base.ExecuteAsync();
        Debug.Log("PlayerAttackCommand executed");
        player.AnimationController.CrossFade(AnimationController.AttackHash, 0.1f);
        // await Task.Delay((int)(attackAnimDuration * 1000));
    }

    public override async Awaitable ExecuteAsync()
    {
        Debug.Log("PlayerAttackCommand executed");
        player.AnimationController.CrossFade(AnimationController.AttackHash, 0.1f);
        await Awaitable.WaitForSecondsAsync(attackAnimDuration);
    }
}