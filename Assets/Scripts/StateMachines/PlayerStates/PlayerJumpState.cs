using TriInspector;
using UnityEngine;

public class PlayerJumpState : BasePlayerState
{
    [ReadOnly][SerializeField] float jumpForce;
    [ReadOnly][SerializeField] float speed;
    public PlayerJumpState(PlayerController player, float speed, float jumpForce) : base(player)
    {
        this.speed = speed;
        this.jumpForce = jumpForce;
    }

    public override void Enter()
    {
        base.Enter();
        player.Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.transform.Translate(speed * Time.fixedDeltaTime * player.Direction);
    }
}