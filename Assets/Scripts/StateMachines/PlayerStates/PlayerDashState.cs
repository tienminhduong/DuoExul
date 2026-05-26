using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerDashState : BasePlayerState
{
    private float dashSpeed;
    private float dashRange;

    public PlayerDashState(PlayerController player, float dashSpeed, float dashRange) : base(player)
    {
        this.dashSpeed = dashSpeed;
        this.dashRange = dashRange;
    }

    public override void Enter()
    {
        base.Enter();
        var walkAnimData = new AnimationData(AnimationData.PriorityLevel.Standard, "Walk");
        var _ = AnimationController.PlayAnimation(walkAnimData);
        player.PerformDash().ContinueWith(() => player.ChangeState<PlayerWalkingState>());
    }
}