using UnityEngine;

public class TaurusIdle: IState
{
    readonly TaurusBoss _b;
    float _timer;

    public TaurusIdle(TaurusBoss b) => _b = b;

    public void Enter()
    {
        _b.rb.linearVelocity = Vector2.zero;
        _b.anim.SetBool("IsWalking", false);
        _timer = _b.config.idleDuration;
    }

    public void Update() => _timer -= Time.deltaTime;
    public void FixedUpdate() { }
    public void Exit() { }
    public bool IsFinished() => _timer <= 0f;
}
