using UnityEngine;

public abstract class BaseAriesState : IState
{
    protected AnimationController animationController;
    protected BaseEnemyController enemyController;
    // This variable is used to indicate whether the state has completed its action
    // and can transition to another state
    // Default value is true because some states is looped and can transition to another state immediately,
    // for example, Idle state can transition to Walk state immediately when the enemy sees the player
    protected bool IsDone = true; 
    public BaseAriesState(AnimationController animationController, 
                          BaseEnemyController enemyController)
    {
        this.animationController = animationController;
        this.enemyController = enemyController;
    }
    public bool IsCompleted() => IsDone;
    virtual public void Enter() { }

    virtual public void Exit() { }

    virtual public void FixedUpdate() { }

    virtual public void Update() { }
    virtual public bool IsFinished() => IsDone;
}
