using UnityEditor;
using UnityEngine;

public class IdleAriesState: BaseAriesState
{
    AnimationData idleAnimData = new(AnimationData.PriorityLevel.Standard, "AriesIdle");
    public IdleAriesState(AnimationController animationController,
                          BaseEnemyController enemyController)
        : base(animationController, enemyController)
    {}

    override public void Enter()
    {
        base.Enter();
        var _ = animationController.PlayAnimation(idleAnimData);
        Debug.Log("Entered Idle State");
    }

    public override void Update()
    {
        base.Update();
        // Transition to other states based on conditions (e.g., player proximity)
        if (enemyController.CanSeePlayer())
        {
            enemyController.ChangeState<MoveAriesState>();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Handle physics-related updates if necessary
    }
}
