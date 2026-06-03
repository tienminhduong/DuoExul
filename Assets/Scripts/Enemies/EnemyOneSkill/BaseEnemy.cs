using AIEnemy;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AttackCommandInvoker))]
public abstract class BaseEnemy : MonoBehaviour, IAttacker, IDamageable
{
    public float BaseAttack => 30f;

    public AnimationController AnimationController => animationController;

    public Rigidbody2D Rigidbody => _rigidbody2D;

    public HealthComponent HealthComponent => healthComponent;

    public virtual void Attack(AttackData attackData)
    {
        
    }

    protected AnimationController animationController;
    protected Rigidbody2D _rigidbody2D;
    protected HealthComponent healthComponent;
    protected AIStateMachine stateMachine;
    protected AttackCommandInvoker commandInvoker;


    private void Awake()
    {
        animationController = new AnimationController(GetComponent<Animator>());
        _rigidbody2D = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponent>();
        commandInvoker = GetComponent<AttackCommandInvoker>();
        stateMachine = new AIStateMachine();
    }

    private void Start()
    {
        InitializeStateMachine();
    }

    protected virtual void InitializeStateMachine()
    {}

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }


}
