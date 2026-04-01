using TriInspector;
using UnityEngine;

public class PlayerWalkingState : BasePlayerState
{
    [ReadOnly][SerializeField] private float speed;
    public PlayerWalkingState(PlayerController player, float speed) : base(player)
    {
        this.speed = speed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.transform.Translate(speed * Time.fixedDeltaTime * player.Direction);
    }
}