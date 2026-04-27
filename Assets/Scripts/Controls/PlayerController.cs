using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerController : MonoBehaviour, IAttacker, IDamageable
{
    public Rigidbody2D Rigidbody { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public int Direction;
    public AnimationController AnimationController { get; private set; }
    public HealthComponent HealthComponent { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private int baseAttack = 10;
    public int BaseAttack => baseAttack;

    [SerializeField] private LayerMask groundLayer;

    [Header("Reference")]
    [SerializeField] private GameObject hitbox;
    [SerializeField] private AttackCommandInvoker attackCommandInvoker;

    [Header("Attack Commands")]
    [SerializeField] private List<CommandData> attackCommands = new List<CommandData>();


    [Header("Debug readonly")]
    public Vector2 CurrentDirectionInput;
    [ReadOnly][SerializeField] private StateMachine stateMachine;

    // For double jumping, reset if touch the ground, -1 when leave the ground
    [ReadOnly][SerializeField] private int viableJumps = 1;


    private bool isGrounded;
    private GroundDetector groundDetector;


    // Actions
    public UnityAction OnGrounded;


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        AnimationController = new AnimationController(GetComponentInChildren<Animator>());
        HealthComponent = GetComponent<HealthComponent>();

        groundDetector = GetComponentInChildren<GroundDetector>();

        SetupStateMachine();
    }

    void OnEnable()
    {
        groundDetector.OnGrounded += OnGroundedCollided;
        groundDetector.OnLeftGround += OnLeftGround;

        EventBus<ChangeHealthPlayerEvent>.Register(OnChangeHealthPlayer);
    }

    void OnDisable()
    {
        groundDetector.OnGrounded -= OnGroundedCollided;
        groundDetector.OnLeftGround -= OnLeftGround;

        EventBus<ChangeHealthPlayerEvent>.Unregister(OnChangeHealthPlayer);
    }

    private void SetupStateMachine()
    {
        stateMachine = new StateMachine();

        stateMachine.AddState(new PlayerIdleState(this));
        stateMachine.AddState(new PlayerWalkingState(this));
        stateMachine.AddState(new PlayerJumpState(this, jumpHeight));
        stateMachine.AddState(new PlayerFallState(this));

        stateMachine.SetState<PlayerIdleState>();
    }

    private void ResetOnTouch()
    {
        viableJumps = maxJumps;
    }

    public void ChangeState<T>() where T : IState
    {
        stateMachine.ChangeState<T>();
    }

    public void SetDirection(Vector2 vector2)
    {
        // Debug.Log($"Input data: {vector2}");
        Direction = Mathf.Abs(vector2.x) >= 0.1f ? (int)Mathf.Sign(vector2.x) : 0;
        transform.localScale = new Vector3(Direction != 0 ? -Direction : transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (Direction != 0 && stateMachine.IsInState<PlayerIdleState>())
            stateMachine.ChangeState<PlayerWalkingState>();
        else if (stateMachine.IsInState<PlayerWalkingState>())
            stateMachine.ChangeState<PlayerIdleState>();
    }

    public void SetJump()
    {
        if (viableJumps <= 0)
            return;

        viableJumps--;
        stateMachine.ChangeState<PlayerJumpState>();
    }

    public void SetFall()
    {
        if (stateMachine.IsInState<PlayerJumpState>())
            stateMachine.ChangeState<PlayerFallState>();
    }

    // called in fixed update
    public void UpdateMoving()
    {
        transform.Translate(Direction * moveSpeed * Time.fixedDeltaTime * Vector2.right);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void OnGroundedCollided()
    {
        ResetOnTouch();
        isGrounded = true;
        OnGrounded?.Invoke();
    }

    private void OnLeftGround()
    {
        isGrounded = false;
    }

    public void Attack(AttackData attackData)
    {
        hitbox.SetActive(true);
    }

    public void HandleAttackInput()
    {
        CommandData selectedAttackCommand = attackCommands[0];
        if (CurrentDirectionInput.y < -0.5f && attackCommands.Count > 1 && !isGrounded)
            selectedAttackCommand = attackCommands[1];
        if (attackCommandInvoker.SetComboAttackData(selectedAttackCommand))
        {
            var _ = attackCommandInvoker.ExecuteCommandsAsync();
        }
    }

    public void HandleAttackInputCancel()
    {
        attackCommandInvoker.PauseExecution();
    }

    private void OnChangeHealthPlayer(ChangeHealthPlayerEvent evt)
    {
        Debug.Log($"Player health change event received: {evt.HealthChange}");
        if (evt.HealthChange < 0)
        {
            HealthComponent.TakeDamage((int)-evt.HealthChange);
        }
        else
        {
            HealthComponent.Heal((int)evt.HealthChange);
        }
    }
}