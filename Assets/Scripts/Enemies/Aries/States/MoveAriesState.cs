using UnityEngine;

public class MoveAriesState : BaseAriesState
{
    private readonly AnimationData moveAnimationData = new AnimationData(AnimationData.PriorityLevel.Standard, "AriesRun");
    private GameObject player = null;
    public MoveAriesState(AnimationController animationController, 
                          BaseEnemyController enemyController) 
        : base(animationController, enemyController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Move State");
        var _ = animationController.PlayAnimation(moveAnimationData);
    }

    public override void Update()
    {
        base.Update();
        // Implement movement logic here (e.g., move towards player)
        if (!enemyController.CanSeePlayer())
        {
            enemyController.ChangeState<IdleAriesState>();
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Handle physics-related updates for movement if necessary
        if(player == null)
        {
            player = enemyController.GetPlayer();
            if(player == null)
            {
                Debug.Log("Player not found, cannot move towards player.");
                return;
            }
        }
        Vector2 moveDirection = (player.transform.position.x > enemyController.transform.position.x) 
                                ? Vector2.right: Vector2.left;
        enemyController.Move(moveDirection);
    }

    public override void Exit()
    {
        base.Exit();
        player = null;
        // Clean up or reset any movement-related variables if necessary
    }
}
