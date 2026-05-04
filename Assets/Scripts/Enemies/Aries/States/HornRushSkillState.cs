using UnityEngine;

public class HornRushSkillState : BaseAriesState
{
    private readonly AnimationData moveAnimationData = new AnimationData(AnimationData.PriorityLevel.Standard, "AriesRun");

    private float duration;
    private float maxSpeed;
    private float currentTime;

    private GameObject player = null;
    public HornRushSkillState(AnimationController animationController, BaseEnemyController enemyController,
                                float duration, float speed) 
        : base(animationController, enemyController)
    {
        this.duration = duration;
        this.maxSpeed = speed;
        currentTime = 0;
    }

    public override void Enter()
    {
        base.Enter();
        IsDone = false;
        Debug.Log("Entered Horn Rush Skill State");
        var _ = animationController.PlayAnimation(moveAnimationData);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        currentTime += Time.fixedDeltaTime;
        if (currentTime >= duration)
        {
            IsDone = true;
            return;
        }
        // Move in the current facing direction
        if(player == null)
        {
            player = enemyController.GetPlayer();
            if(player == null)
            {
                Debug.Log("Player not found, cannot perform Horn Rush.");
                IsDone = true; // End the skill if player is not found
                return;
            }
        }
        Vector2 moveDirection = (player.transform.position.x > enemyController.transform.position.x)
                                ? Vector2.right : Vector2.left;
        float speed = CalculateVelocity(currentTime, duration, maxSpeed);
        Debug.Log("Speed: " + speed);
        enemyController.Move(moveDirection, speed);
    }

    private float CalculateVelocity(float currentTime, float duration, float maxSpeed)
    {
        float phase1End = duration * 0.75f; // Thời điểm hết 3/4 thời gian

        if (currentTime <= phase1End)
        {
            // Giai đoạn tăng tốc: t / (0.75 * T)
            return maxSpeed * (currentTime / phase1End);
        }
        else
        {
            // Giai đoạn giảm tốc: 1 - (t - 0.75*T) / (0.25*T)
            float phase2Time = currentTime - phase1End;
            float phase2Duration = duration - phase1End;
            return maxSpeed * (1f - (phase2Time / phase2Duration));
        }
    }
    public override void Exit()
    {
        base.Exit();
        // Reset any variables or states related to the skill if necessary
        currentTime = 0;
        //((AriesController)enemyController).ResetCoolDownSkill1();
    }
}
