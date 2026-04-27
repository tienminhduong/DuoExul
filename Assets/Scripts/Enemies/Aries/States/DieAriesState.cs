using System;
using UnityEditor;
using UnityEngine;

public class DieAriesState : BaseAriesState
{
    readonly AnimationData dieAnimation = new AnimationData(AnimationData.PriorityLevel.Standard, "AriesDie");
    public DieAriesState(AnimationController animationController, BaseEnemyController enemyController) : base(animationController, enemyController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Die State");
        var _ =  animationController.PlayAnimation(dieAnimation);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting Die State");
    }
}
