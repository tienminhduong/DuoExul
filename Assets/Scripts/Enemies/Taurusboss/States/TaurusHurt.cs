using UnityEngine;

// ══════════════════════════════════════════════════════════════════════════
//  HURT
//  Stagger ngắn khi nhận damage, sau đó tiếp tục Walk.
// ══════════════════════════════════════════════════════════════════════════
public class TaurusHurt : IState
{
    readonly TaurusBoss _b;
    float _timer;

    public TaurusHurt(TaurusBoss b) => _b = b;

    public void Enter()
    {
        _timer = _b.config.hurtDuration;
        _b.rb.linearVelocity = Vector2.zero;
        _b.anim.SetTrigger("Hurt");
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f) _b.ClearHurt();
    }

    public void FixedUpdate() { }
    public void Exit() { }
    public bool IsFinished() => !_b.IsHurt;
}