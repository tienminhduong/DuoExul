using UnityEngine;

// ══════════════════════════════════════════════════════════════════════════
//  SKILL 1 – CHARGE
//  Phase: Windup (cúi đầu) → Charging (lao) → Recover (dừng + pause)
// ══════════════════════════════════════════════════════════════════════════
public class TaurusCharge : IState
{
    readonly TaurusBoss _b;

    enum Phase { Windup, Charging, Recover }
    Phase _phase;
    float _timer;
    float _chargeDir;
    bool _done;

    public TaurusCharge(TaurusBoss b) => _b = b;

    public void Enter()
    {
        _done = false;
        _phase = Phase.Windup;
        _timer = _b.config.chargeWindup;
        _chargeDir = _b.DirToPlayer();   // lock hướng khi bắt đầu
        _b.FacePlayer();
        _b.rb.linearVelocity = Vector2.zero;
        _b.cooldowns.Use(CD);
        _b.anim.SetTrigger("ChargeWindup");

        // Tắt hitbox charge khi mới vào
        if (_b.chargeHitbox) _b.chargeHitbox.enabled = false;
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        switch (_phase)
        {
            case Phase.Windup when _timer <= 0f:
                _phase = Phase.Charging;
                _timer = _b.config.chargeDuration;
                _b.anim.SetTrigger("ChargeStart");
                if (_b.chargeHitbox) _b.chargeHitbox.enabled = true;
                break;

            case Phase.Charging when _timer <= 0f:
                _phase = Phase.Recover;
                _timer = _b.config.postSkillPause;
                _b.rb.linearVelocity = Vector2.zero;
                if (_b.chargeHitbox) _b.chargeHitbox.enabled = false;
                _b.anim.SetTrigger("ChargeEnd");
                break;

            case Phase.Recover when _timer <= 0f:
                _done = true;
                break;
        }
    }

    public void FixedUpdate()
    {
        if (_phase == Phase.Charging)
            _b.rb.linearVelocity = new Vector2(_chargeDir * _b.config.chargeSpeed, _b.rb.linearVelocityY);
    }

    public void Exit()
    {
        if (_b.chargeHitbox) _b.chargeHitbox.enabled = false;
    }

    public bool IsFinished() => _done;

    const string CD = TaurusBoss.CD_CHARGE;
}