using UnityEngine;

public abstract class BaseEnemyState : IState
{
    protected BaseEnemyController enemyController;
    protected Animator animator;
    protected string animationName; // Name of the animation to play when entering this state
    protected const float crossFadeDuration = 0.1f; // Duration for animation crossfade

    protected BaseEnemyState(BaseEnemyController enemyController, Animator animator, string animationName)
    {
        this.enemyController = enemyController;
        this.animator = animator;
        this.animationName = animationName;
    }
    public virtual void Enter()
    {
        // noop
    }

    public virtual void Exit()
    {
        // noop
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void Update()
    {
        // noop
    }

    public virtual bool IsFinished()
    {
        return false; // By default, states are not finished until explicitly set otherwise
    }
}
