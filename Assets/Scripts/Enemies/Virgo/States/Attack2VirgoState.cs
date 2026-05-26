using UnityEngine;
using Utilities;

public class Attack2VirgoState : BaseEnemyState
{
    CountdownTimer cooldownTimer;
    public Attack2VirgoState(BaseEnemyController enemyController, Animator animator, string animationName, CountdownTimer cooldownTimer) 
        : base(enemyController, animator, animationName)
    {
        this.cooldownTimer = cooldownTimer;
    }

    public override void Enter()
    {
        base.Enter();
        animator.CrossFade(animationName, crossFadeDuration);
        cooldownTimer.Reset();
    }

    public override void Exit()
    {
        base.Exit();
        cooldownTimer.Start();
    }

    public override bool IsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.IsName(animationName) &&
                stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }
}
