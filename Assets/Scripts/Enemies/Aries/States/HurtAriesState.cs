using System;
using UnityEngine;

public class HurtAriesState : BaseEnemyState
{
    public HurtAriesState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hurt");
        animator.CrossFade(animationName, crossFadeDuration);
    }

    public override void Exit()
    {
        base.Exit();
        enemyController.SetHurt(false);
    }

    public override bool IsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.IsName(animationName) && 
                stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }
}
