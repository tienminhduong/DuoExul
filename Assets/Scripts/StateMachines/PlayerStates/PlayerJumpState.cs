using TriInspector;
using UnityEngine;

public class PlayerJumpState : BasePlayerState
{
    [ReadOnly][SerializeField] float jumpHeight;
    public PlayerJumpState(PlayerController player, float jumpHeight) : base(player)
    {
        this.jumpHeight = jumpHeight;
    }

    public override void Enter()
    {
        base.Enter();
        player.Rigidbody.linearVelocityY = 0;
        var jumpForce = Mathf.Sqrt(2 * jumpHeight * -Physics2D.gravity.y * player.Rigidbody.gravityScale) * player.Rigidbody.mass;
        player.Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // AnimationController.CrossFade(AnimationController.JumpHash, 0.1f);
        var jumpAnimData = new AnimationData(AnimationData.PriorityLevel.Standard, "Jump", 0.1f);
        var _ = AnimationController.PlayAnimation(jumpAnimData);
    }

    public override void Update()
    {
        base.Update();
        if (player.Rigidbody.linearVelocityY < 0)
            player.ChangeState<PlayerFallState>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.UpdateMoving();
    }
}