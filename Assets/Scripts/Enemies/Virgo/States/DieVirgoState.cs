using UnityEngine;

public class DieVirgoState : BaseEnemyState
{
    public DieVirgoState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Die");
        animator.CrossFade(animationName, crossFadeDuration);
        enemyController.SetDie();
    }
}
