using UnityEditor;
using UnityEngine;

public class IdleAriesState: BaseEnemyState
{
    int idleHash;
    public IdleAriesState(BaseEnemyController enemyController, 
                          Animator animator,
                          string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Idle");
        animator.CrossFade(animationName, crossFadeDuration);
    }
}
