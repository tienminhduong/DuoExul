using UnityEditor;
using UnityEngine;

public class Attack1VirgoState : BaseEnemyState
{
    public Attack1VirgoState(BaseEnemyController enemyController, Animator animator, string animationName)
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("[Attack1VirgoState] Enter");
        animator.CrossFade(animationName, crossFadeDuration);
    }

    public override bool IsFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.IsName(animationName) &&
                stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }
}
