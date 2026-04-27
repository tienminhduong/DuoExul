using UnityEngine;

public abstract class BaseAriesState : IState
{
    protected AnimationController animationController;
    protected BaseEnemyController enemyController;
    public BaseAriesState(AnimationController animationController, 
                          BaseEnemyController enemyController)
    {
        this.animationController = animationController;
        this.enemyController = enemyController;
    }
    virtual public void Enter() { }

    virtual public void Exit() { }

    virtual public void FixedUpdate() { }

    virtual public void Update() { }
}
