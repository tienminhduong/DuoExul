using AIEnemy;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class VirgoController : BaseEnemyController, IEntity, IDamageable
{
    [SerializeField] PlayerDetector playerDetector;

    [Header("Skill1")]
    [SerializeField] private Transform skill1SpawnPoint;
    [SerializeField] private float coolDownSkill1 = 10f;
    [SerializeField] private float distanceToUseSkill1 = 3f;
    [SerializeField] private float percentageChanceToUseSkill1 = 0.5f;

    [Header("Skill2")]
    [SerializeField] private float coolDownSkill2 = 10f;
    [SerializeField] private Transform offsetSpawnEarth;
    [SerializeField] private float distanceToUseSkill2 = 5f;
    [SerializeField] private float percentageChanceToUseSkill2 = 0.5f;

    [Header("Protect")]
    [SerializeField] private float distanceToRunAway = 2f;

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
        timerSkill1 = new CountdownTimer(coolDownSkill1);
        timerSkill2 = new CountdownTimer(coolDownSkill2);

        // Khai báo states
        var idleState = new IdleVirgoState(this, animator, "VirgoIdle");
        var runState = new RunVirgoState(this, animator, "VirgoRun");
        var skill1State = new Attack1VirgoState(this, animator, "VirgoSkill1", timerSkill1);
        var skill2State = new Attack2VirgoState(this, animator, "VirgoSkill2", timerSkill2);
        var hurtState = new HurtVirgoState(this, animator, "VirgoHurt");
        var dieState = new DieVirgoState(this, animator, "VirgoDie");

        At(idleState, hurtState, new FuncPredicate(() => IsHurt()));
        At(idleState, runState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));

        At(runState, idleState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(runState, hurtState, new FuncPredicate(() => IsHurt()));
        At(runState, skill1State, new FuncPredicate(() => CheckConditionTransitionFromMoveToSkill1()));
        At(runState, skill2State, new FuncPredicate(() => CheckConditionTransitionFromMoveToSkill2()));

        At(hurtState, idleState, new FuncPredicate(() => hurtState.IsFinished()));
        At(hurtState, dieState, new FuncPredicate(() => HealthComponent.CurrentHealth <= 0));

        At(skill1State, idleState, new FuncPredicate(() => skill1State.IsFinished()));
        At(skill1State, hurtState, new FuncPredicate(() => IsHurt()));

        At(skill2State, idleState, new FuncPredicate(() => skill2State.IsFinished()));
        At(skill2State, hurtState, new FuncPredicate(() => IsHurt()));
        stateMachine.SetState(idleState);
    }

    bool CheckConditionTransitionFromMoveToSkill1()
    {
        var player = playerDetector.Player;
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance < distanceToUseSkill1 && timerSkill1.IsFinished && 
                Random.Range(0.0f, 1.0f) < percentageChanceToUseSkill1;
    }    

    bool CheckConditionTransitionFromMoveToSkill2()
    {
        var player = playerDetector.Player;
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance < distanceToUseSkill2 && timerSkill2.IsFinished && 
                Random.Range(0.0f, 1.0f) < percentageChanceToUseSkill2;
    }    

    private void Update()
    {
        stateMachine.Update();
        timerSkill1.Tick(Time.deltaTime);
        timerSkill2.Tick(Time.deltaTime);
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

    public void SpawnEarth()
    {
        var player = playerDetector.Player;
        if(player == null) return;

        Vector2 spawnPosition = new Vector2(player.transform.position.x,
            offsetSpawnEarth.position.y);
            
        ProjectileSystem.Instance.Spawn(ProjectileType.VirgoEarth, spawnPosition, Quaternion.identity, Vector2.down);
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Debug.Log("Virgo hit by player attack");
            SetHurt(true);
        }
    }

    public override GameObject GetPlayer()
    {
        return playerDetector.Player;
    }  

    public override void SetDie()
        {
            base.SetDie();
            _collider.enabled = false;
            _rigidbody.gravityScale = 0;
    }

    public Vector2 CalculateVectorMove()
    {
        var player = playerDetector.Player;
        if(player == null) return Vector2.zero;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        
        if(distance <= distanceToRunAway || (!timerSkill1.IsFinished && !timerSkill2.IsFinished))
        {
            // run away
            return (transform.position.x > player.transform.position.x) ? Vector2.right : Vector2.left;
        }
        else
        {
            // run to player
            return (transform.position.x > player.transform.position.x) ? Vector2.left : Vector2.right;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToUseSkill1);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanceToUseSkill2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceToRunAway);
    }
}
