using AIEnemy;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Zombie : MonoBehaviour, IAttacker
{
    // Animation name
    public const string WALK_ANIMATION = "Walk";
    public const string ATTACK_ANIMATION = "Attack";
    public const string DIE_ANIMATION = "Die";
    public const string HURT_ANIMATION = "Hurt";
    public const string IDLE_ANIMATION = "Idle";

    AIStateMachine stateMachine;

    public float BaseAttack => 10f;

    public AnimationController AnimationController => animationController;

    public Rigidbody2D Rigidbody => _rigidbody2D;


    AttackCommandInvoker commandInvoker;
    Rigidbody2D _rigidbody2D;
    AnimationController animationController;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animationController = new AnimationController(GetComponent<Animator>());
        commandInvoker = GetComponent<AttackCommandInvoker>();
    }

    private void Start()
    {
        InitializeStateMachine();
    }

    void InitializeStateMachine()
    {
        stateMachine = new AIStateMachine();

    }

    private void Update()
    {
        stateMachine.Update();
        commandInvoker.ExecuteCommandsAsync().Forget();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void Attack(AttackData attackData)
    {

    }
}
