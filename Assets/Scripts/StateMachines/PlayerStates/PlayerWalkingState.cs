public class PlayerWalkingState : BasePlayerState
{
    public PlayerWalkingState(PlayerController player, float speed) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.HandleMoving();
    }
}