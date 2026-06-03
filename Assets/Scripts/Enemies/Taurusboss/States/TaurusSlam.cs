using UnityEngine;

// ══════════════════════════════════════════════════════════════════════════
//  SKILL 2 – GROUND SLAM
//  Phase: Windup → Hit (spawn shockwave) → Recover
// ══════════════════════════════════════════════════════════════════════════
public class TaurusSlam : IState
{
    readonly TaurusBoss _b;

    enum Phase { Windup, Hit, Recover }
    Phase _phase;
    float _timer;
    bool _done;

    public TaurusSlam(TaurusBoss b) => _b = b;

    public void Enter()
    {
        _done = false;
        _phase = Phase.Windup;
        _timer = _b.config.slamWindup;
        _b.rb.linearVelocity = Vector2.zero;
        _b.FacePlayer();
        _b.cooldowns.Use(CD);
        _b.anim.SetTrigger("SlamWindup");
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        switch (_phase)
        {
            case Phase.Windup when _timer <= 0f:
                _phase = Phase.Hit;
                _b.anim.SetTrigger("SlamHit");
                _b.SpawnShockwave(_b.config.slamShockwaveW, _b.config.slamDamage);
                _timer = _b.config.postSkillPause;
                break;

            case Phase.Hit when _timer <= 0f:
                _phase = Phase.Recover;
                _done = true;
                break;
        }
    }

    public void FixedUpdate() { }
    public void Exit() { }
    public bool IsFinished() => _done;

    const string CD = TaurusBoss.CD_SLAM;
}