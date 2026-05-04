using UnityEngine;

public class MoveAriesState : BaseEnemyState
{
    Transform playerTransform;
    public MoveAriesState(BaseEnemyController enemyController, Animator animator, 
        string animationName, Transform playerTransform) 
        : base(enemyController, animator, animationName)
    {
        this.playerTransform = playerTransform;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Move");
        animator.CrossFade(animationName, crossFadeDuration);
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 direction = (playerTransform.position.x > enemyController.transform.position.x) 
                            ? Vector2.right : Vector2.left;
        enemyController.Move(direction);
    }
}
