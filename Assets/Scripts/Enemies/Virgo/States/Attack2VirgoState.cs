using UnityEngine;

public class Attack2VirgoState : BaseEnemyState
{
    public Attack2VirgoState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animator.CrossFade(animationName, crossFadeDuration);
        ((VirgoController)enemyController).SpawnEarth();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override bool IsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.IsName(animationName) &&
                stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }
}
