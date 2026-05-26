using UnityEditor;
using UnityEngine;
using Utilities;

public class Attack1VirgoState : BaseEnemyState
{
    CountdownTimer cooldownTimer;
    public Attack1VirgoState(BaseEnemyController enemyController, Animator animator, string animationName, CountdownTimer countdownTimer)
        : base(enemyController, animator, animationName)
    {
        this.cooldownTimer = countdownTimer;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("[Attack1VirgoState] Enter");
        animator.CrossFade(animationName, crossFadeDuration);
        cooldownTimer.Reset();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("[Attack1VirgoState] Exit");
        cooldownTimer.Start();
    }

    public override bool IsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.IsName(animationName) &&
                stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }
}
