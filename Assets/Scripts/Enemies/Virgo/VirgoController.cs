using AIEnemy;
using UnityEngine;
using Utilities;

public class VirgoController : BaseEnemyController, IEntity, IDamageable
{
    [SerializeField] PlayerDetector playerDetector;

    [Header("Skill1")]
    [SerializeField] private Transform skill1SpawnPoint;

    [Header("Skill2")]

    Animator animator;
    AIStateMachine stateMachine;
    CountdownTimer timerSkill1;
    CountdownTimer timerSkill2;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        stateMachine = new AIStateMachine();
        // khai báo timer cho các skill
        timerSkill1 = new CountdownTimer(10f);
        timerSkill2 = new CountdownTimer(10f);

        // Khai báo states
        var idleState = new IdleVirgoState(this, animator, "VirgoIdle");
        var runState = new RunVirgoState(this, animator, "VirgoRun");
        var skill1State = new Attack1VirgoState(this, animator, "VirgoSkill1");
        var hurtState = new HurtVirgoState(this, animator, "VirgoHurt");

        //At(idleState, skill1State, new FuncPredicate(() => Random.value < 0.5f));
        //At(skill1State, idleState, new FuncPredicate(() => skill1State.IsFinished()));
        At(idleState, hurtState, new FuncPredicate(() => IsHurt()));
        //At(skill1State, hurtState, new FuncPredicate(() => IsHurt()));
        At(hurtState, idleState, new FuncPredicate(() => hurtState.IsFinished()));
        stateMachine.SetState(idleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    public void SpawnProjectile()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        ProjectileSystem.Instance.Spawn(ProjectileType.VirgoProjectile, skill1SpawnPoint.position, Quaternion.identity, direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Debug.Log("Virgo hit by player attack");
            SetHurt(true);
        }
    }
}
