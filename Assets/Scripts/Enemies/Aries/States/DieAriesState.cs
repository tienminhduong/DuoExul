using System;
using UnityEditor;
using UnityEngine;

public class DieAriesState : BaseEnemyState
{
    public DieAriesState(BaseEnemyController enemyController, Animator animator, string animationName) 
        : base(enemyController, animator, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Die");
        animator.CrossFade(animationName, crossFadeDuration);
        enemyController.SetDie();
    }
}
