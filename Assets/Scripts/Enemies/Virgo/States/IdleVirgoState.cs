using UnityEngine;

public class IdleVirgoState : BaseEnemyState
{
    public IdleVirgoState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Th?c hi?n các hành ??ng khi vào tr?ng thái Idle, ví d?: reset timer, set animation, v.v.
        this.animator.Play(animationName);
        Debug.Log("[IdleVirgoState] Enter");
    }
}
