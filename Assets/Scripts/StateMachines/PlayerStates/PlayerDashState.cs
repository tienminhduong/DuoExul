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
        var dashAnimData = new AnimationData(AnimationData.PriorityLevel.Standard, "Dash");
        var _ = AnimationController.PlayAnimation(dashAnimData);
        player.PerformDash().Forget();
    }
}