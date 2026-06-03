using UnityEngine;

// ══════════════════════════════════════════════════════════════════════════
//  SKILL 3 – STOMP BARRAGE  (Phase 2 only)
//  Giậm stompCount lần, mỗi lần spawn shockwave nhỏ.
// ══════════════════════════════════════════════════════════════════════════
public class TaurusStomp : IState
{
    readonly TaurusBoss _b;

    int _stompsDone;
    float _timer;
    bool _done;

    public TaurusStomp(TaurusBoss b) => _b = b;

    public void Enter()
    {
        _done = false;
        _stompsDone = 0;
        _timer = _b.config.stompInterval;
        _b.rb.linearVelocity = Vector2.zero;
        _b.cooldowns.Use(CD);
        _b.anim.SetTrigger("StompStart");
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        if (_stompsDone < _b.config.stompCount)
        {
            _b.anim.SetTrigger("StompHit");
            _b.SpawnShockwave(_b.config.stompShockwaveW, _b.config.stompDamage);
            _stompsDone++;
            _timer = _b.config.stompInterval;
        }
        else
        {
            _done = true;
        }
    }

    public void FixedUpdate() { }
    public void Exit() { }
    public bool IsFinished() => _done;

    const string CD = TaurusBoss.CD_STOMP;
}
