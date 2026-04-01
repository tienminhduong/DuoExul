public class PlayerIdleState : BasePlayerState
{
    public PlayerIdleState(PlayerController player) : base(player)
    {
        
    }

    public override void Update()
    {
        base.Update();
        if (player.Direction.magnitude > 0)
            player.ChangeState<PlayerWalkingState>();
    }
}