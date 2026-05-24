using UnityEngine;
using Utilities;

public class HornRushSkillState : BaseEnemyState
{
    GameObject player;
    Vector2 direction;
    CountdownTimer finishStateTimer;
    CountdownTimer cooldownTimer;
    public HornRushSkillState(BaseEnemyController enemyController, Animator animator,
                              string animationName, float timeAttack,
                              CountdownTimer coolDownTimer) 
        : base(enemyController, animator, animationName)
    {
        finishStateTimer = new CountdownTimer(timeAttack);
        this.cooldownTimer = coolDownTimer;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Horn Rush Skill State");
        animator.Play(animationName);
        player = enemyController.GetPlayer();
        if (player == null)
        {
            Debug.LogWarning("Player not found. Horn Rush Skill State will not move.");
        }
        else
        {
            Debug.Log("Player found. Horn Rush Skill State will move towards the player.");
            direction = (player.transform.position.x > enemyController.transform.position.x) ? Vector2.right : Vector2.left;
        }
        finishStateTimer.Start();
        cooldownTimer.Stop();
        cooldownTimer.Reset();
    }

    public override void Update()
    {
        base.Update();
        finishStateTimer.Tick(Time.deltaTime);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(player == null) return;

        enemyController.Move(direction, 10); // Move towards the player at a speed of 10 units per second
    }

    public override void Exit()
    {
        base.Exit();
        player = null;
        direction = Vector2.zero;
        finishStateTimer.Reset();
        cooldownTimer.Start();
    }

    public override bool IsFinished()
    {
        return finishStateTimer.IsFinished;
    }
}
