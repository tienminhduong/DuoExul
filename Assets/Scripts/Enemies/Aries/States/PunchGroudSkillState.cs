using UnityEngine;
using Utilities;

public class PunchGroudSkillState : BaseEnemyState
{
    CountdownTimer cooldownTimer;
    public PunchGroudSkillState(BaseEnemyController enemyController, Animator animator, 
                                string animationName, CountdownTimer cooldownTimer) 
        : base(enemyController, animator, animationName)
    {
        this.cooldownTimer = cooldownTimer;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Punch Ground Skill State");
        animator.Play(animationName);
        cooldownTimer.Stop();
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
