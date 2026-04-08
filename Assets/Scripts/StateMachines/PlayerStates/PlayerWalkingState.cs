using TriInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkingState : BasePlayerState
{
    public PlayerWalkingState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        var walkAnimData = new AnimationData(AnimationData.PriorityLevel.Standard, "Walk");
        var _ = AnimationController.PlayAnimation(walkAnimData);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.UpdateMoving();
    }
}