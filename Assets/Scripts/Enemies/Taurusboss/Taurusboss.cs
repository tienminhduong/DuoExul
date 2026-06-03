using AIEnemy;
using UnityEngine;

/// <summary>
/// Kim Ngưu (Taurus) – Boss cận chiến hung hăng, không cho player thở.
///
///  State Graph:
///  Idle ──► Walk ──► [Charge | Slam | Stomp(P2)]
///  AnyState ──► Hurt (ưu tiên tuyệt đối)
///
///  Skill 1 – Charge       : CD 5s   – lao thẳng từ xa, knockback mạnh
///  Skill 2 – Ground Slam  : CD 7s   – đập tại chỗ, spawn shockwave
///  Skill 3 – Stomp Barrage: CD 10s  – Phase 2 only, giậm 3 lần liên tiếp
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class TaurusBoss : MonoBehaviour
{
    // ── Inspector ────────────────────────────────────────────────────────
    [Header("Config")]
    public TaurusConfig config;

    [Header("References")]
    public Transform player;
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public SpriteRenderer sr { get; private set; }

    [Header("Hitbox / VFX")]
    [Tooltip("Child collider kích hoạt khi đang Charge (isTrigger = true, disabled mặc định)")]
    public Collider2D chargeHitbox;
    [Tooltip("Prefab shockwave (Shockwave.cs, Collider2D isTrigger)")]
    public GameObject shockwavePrefab;
    [Tooltip("Transform ở chân boss – nơi spawn shockwave")]
    public Transform groundPoint;

    // ── Stats ────────────────────────────────────────────────────────────
    public float CurrentHP { get; private set; }
    public bool IsPhase2 { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsHurt { get; private set; }

    // ── Cooldowns ────────────────────────────────────────────────────────
    public SkillCooldowns cooldowns { get; private set; }

    public const string CD_CHARGE = "charge";
    public const string CD_SLAM = "slam";
    public const string CD_STOMP = "stomp";

    // ── State Machines ───────────────────────────────────────────────────
    AIStateMachine _sm;

    // ── State instances ──────────────────────────────────────────────────
    TaurusIdle _stIdle;
    TaurusWalk _stWalk;
    TaurusCharge _stCharge;
    TaurusSlam _stSlam;
    TaurusStomp _stStomp;
    TaurusHurt _stHurt;

    // ════════════════════════════════════════════════════════════════════
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (config == null) config = ScriptableObject.CreateInstance<TaurusConfig>();
        CurrentHP = 2;//config.maxHP;

        cooldowns = new SkillCooldowns();
        cooldowns.Register(CD_CHARGE, config.chargeCooldown);
        cooldowns.Register(CD_SLAM, config.slamCooldown);
        cooldowns.Register(CD_STOMP, config.stompCooldown);
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        BuildStateMachine();
    }

    void BuildStateMachine()
    {
        _sm = new AIStateMachine();

        // ── Tạo states ───────────────────────────────────────────────────
        _stIdle = new TaurusIdle(this);
        _stWalk = new TaurusWalk(this);
        _stCharge = new TaurusCharge(this);
        _stSlam = new TaurusSlam(this);
        _stStomp = new TaurusStomp(this);
        _stHurt = new TaurusHurt(this);

        // ── AnyState → Hurt (ưu tiên tuyệt đối) ─────────────────────────
        _sm.AddAnyTransition(_stHurt,
            new FuncPredicate(() => IsHurt && !IsDead));

        // ── AnyState → Stomp Barrage (Phase 2, dùng khi cận chiến) ──────
        _sm.AddAnyTransition(_stStomp,
            new FuncPredicate(() =>
                !IsHurt && !IsDead && IsPhase2 &&
                cooldowns.IsReady(CD_STOMP) &&
                DistToPlayer() <= config.meleeRange));

        // ── AnyState → Ground Slam (khi player quá gần) ──────────────────
        _sm.AddAnyTransition(_stSlam,
            new FuncPredicate(() =>
                !IsHurt && !IsDead &&
                cooldowns.IsReady(CD_SLAM) &&
                DistToPlayer() <= config.meleeRange));

        // ── AnyState → Charge (khi player đủ xa) ─────────────────────────
        _sm.AddAnyTransition(_stCharge,
            new FuncPredicate(() =>
                !IsHurt && !IsDead &&
                cooldowns.IsReady(CD_CHARGE) &&
                DistToPlayer() >= config.chargeMinRange &&
                DistToPlayer() <= config.detectionRange));

        // ── Idle → Walk ───────────────────────────────────────────────────
        _sm.AddTransition(_stIdle, _stWalk,
            new FuncPredicate(() =>
                _stIdle.IsFinished() &&
                DistToPlayer() <= config.detectionRange));

        // ── Walk → Idle (player bỏ chạy quá xa) ──────────────────────────
        _sm.AddTransition(_stWalk, _stIdle,
            new FuncPredicate(() => DistToPlayer() > config.detectionRange));

        // ── Skill → Walk (sau khi skill xong) ────────────────────────────
        _sm.AddTransition(_stCharge, _stWalk, new FuncPredicate(() => _stCharge.IsFinished()));
        _sm.AddTransition(_stSlam, _stWalk, new FuncPredicate(() => _stSlam.IsFinished()));
        _sm.AddTransition(_stStomp, _stWalk, new FuncPredicate(() => _stStomp.IsFinished()));
        _sm.AddTransition(_stHurt, _stWalk, new FuncPredicate(() => _stHurt.IsFinished()));

        _sm.SetState(_stSlam);
    }

    // ════════════════════════════════════════════════════════════════════
    void Update()
    {
        if (IsDead) return;
        cooldowns.Tick(Time.deltaTime);
        _sm.Update();
    }

    void FixedUpdate()
    {
        if (IsDead) return;
        _sm.FixedUpdate();
    }

    // ── Damage ───────────────────────────────────────────────────────────
    public void TakeDamage(float dmg)
    {
        if (IsDead) return;
        CurrentHP = Mathf.Max(0f, CurrentHP - dmg);

        IsHurt = true;   // AnyTransition → Hurt sẽ xử lý

        if (!IsPhase2 && CurrentHP / config.maxHP <= config.phase2HPRatio)
            TriggerPhase2();

        if (CurrentHP <= 0f)
            Die();
    }

    void TriggerPhase2()
    {
        IsPhase2 = true;
        cooldowns.ScaleAll(config.p2CooldownMult);
        anim.SetTrigger("Phase2Roar");
        Debug.Log("[Taurus] PHASE 2 � RAGE!");
    }

    void Die()
    {
        IsDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("Death");
    }

    // ── Public helpers cho States ────────────────────────────────────────
    public void ClearHurt() => IsHurt = false;

    public float DistToPlayer()
        => player ? Vector2.Distance(transform.position, player.position) : 9999f;

    public float DirToPlayer()
        => player ? Mathf.Sign(player.position.x - transform.position.x) : 1f;

    public void FacePlayer()
    {
        if (player) sr.flipX = player.position.x < transform.position.x;
    }

    public float WalkSpeed => IsPhase2
        ? config.walkSpeed + config.p2SpeedAdd
        : config.walkSpeed;

    /// <summary>Spawn shockwave prefab tại groundPoint với chiều rộng tùy chỉnh.</summary>
    public void SpawnShockwave(float width, float dmg)
    {
        if (shockwavePrefab == null || groundPoint == null) return;
        var go = Instantiate(shockwavePrefab, groundPoint.position, Quaternion.identity);
        go.transform.localScale = new Vector3(width, 1f, 1f);
        //var sw = go.GetComponent<SharedVFX.Shockwave>();
        //if (sw) sw.damage = dmg;
    }
}