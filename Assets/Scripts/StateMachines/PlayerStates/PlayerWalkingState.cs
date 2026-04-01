using TriInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkingState : BasePlayerState
{
    public PlayerWalkingState(PlayerController player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.UpdateMoving();
    }
}