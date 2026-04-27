using System;
using UnityEngine;

public class HurtAriesState : BaseAriesState
{
    readonly AnimationData hurtAnimationData = new AnimationData(AnimationData.PriorityLevel.Standard, "AriesHurt");
    public HurtAriesState(AnimationController animationController, BaseEnemyController enemyController) : base(animationController, enemyController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Hurt State");
        // Play hurt animation
        var _ = animationController.PlayAnimation(hurtAnimationData);
        animationController.OnStandardAnimationComplete += OnHurtAnimationComplete;
    }

    private void OnHurtAnimationComplete()
    {
        enemyController.ChangeState<IdleAriesState>();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting Hurt State");
        animationController.OnStandardAnimationComplete -= OnHurtAnimationComplete;
    }
}
