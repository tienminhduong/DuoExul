using UnityEngine;

public class TaurusWalk: IState
{
    readonly TaurusBoss _b;

    public TaurusWalk(TaurusBoss b) => _b = b;

    public void Enter()
    {
        _b.anim.SetBool("IsWalking", true);
    }

    public void Update() => _b.FacePlayer();

    public void FixedUpdate()
    {
        float dist = _b.DistToPlayer();
        if (dist > _b.config.meleeRange)
            _b.rb.linearVelocity = new Vector2(_b.DirToPlayer() * _b.WalkSpeed, _b.rb.linearVelocityY);
        else
            _b.rb.linearVelocity = new Vector2(0f, _b.rb.linearVelocityY);
    }

    public void Exit() => _b.anim.SetBool("IsWalking", false);
    public bool IsFinished() => false; // Walk không tự kết thúc
}
