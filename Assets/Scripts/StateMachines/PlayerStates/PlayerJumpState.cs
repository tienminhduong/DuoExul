using TriInspector;
using UnityEngine;

public class PlayerJumpState : BasePlayerState
{
    [ReadOnly][SerializeField] float jumpForce;
    public PlayerJumpState(PlayerController player, float jumpForce) : base(player)
    {
        this.jumpForce = jumpForce;
    }

    public override void Enter()
    {
        base.Enter();
        player.Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();
        if (player.Rigidbody.linearVelocityY <= 0)
            player.ChangeState<PlayerFallState>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.UpdateMoving();
    }
}