using UnityEngine;

public class HurtVirgoState : BaseEnemyState
{
    public HurtVirgoState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animator.CrossFade(animationName, crossFadeDuration);
        Debug.Log("[HurtVirgoState] Enter");
    }

    public override bool IsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.IsName(animationName) &&
                stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("[HurtVirgoState] Exit");
        enemyController.SetHurt(false);
    }
}
