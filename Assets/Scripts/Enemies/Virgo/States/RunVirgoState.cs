using UnityEngine;

public class RunVirgoState : BaseEnemyState
{
    public RunVirgoState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
            base.Enter();
        // B?t ??u ch?y
        animator.CrossFade(animationName, crossFadeDuration);
        Debug.Log("[RunVirgoState] Enter");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        enemyController.Move(Vector2.right, 1);
    }

    public override void Exit()
    {
        base.Exit();
        // D?ng ch?y
        Debug.Log("[RunVirgoState] Exit");
    }
}
