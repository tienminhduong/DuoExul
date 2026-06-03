using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    public int FacingDirection => MoveDirection != 0 ? MoveDirection : (transform.localScale.x > 0 ? 1 : -1);
    public AnimationController AnimationController { get; private set; }
    public HealthComponent HealthComponent { get; private set; }
    public PlayerStat playerStat = new();
    public float BaseAttack => playerStat.baseAttack;

    [SerializeField] private LayerMask groundLayer;

    [Header("Reference")]
    [SerializeField] private GameObject hitbox;
    [SerializeField] private AttackCommandInvoker attackCommandInvoker;
    [SerializeField] private List<PlayerWeaponData> weapons = new();

    [ReadOnly] public PlayerWeaponData currentWeapon;

    [Header("Attack Commands")]
    [SerializeField] private List<CommandData> attackCommands = new();
    [ReadOnly][SerializeField] public CommandData defaultAttackCommand;
    [ReadOnly][SerializeField] public CommandData pogoAttackCommand;
    [ReadOnly][SerializeField] public CommandData skillAttackCommand;
    public List<CommandData> AttackCommands => attackCommands;


    [Header("Debug readonly")]
    [ReadOnly] public int MoveDirection;
    [ReadOnly] public Vector2 CurrentDirectionInput;
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
        stateMachine.AddState(new PlayerJumpState(this, playerStat.jumpHeight));
        stateMachine.AddState(new PlayerFallState(this));
        stateMachine.AddState(new PlayerDashState(this, playerStat.dashSpeed, playerStat.dashRange));

        stateMachine.SetState<PlayerIdleState>();
    }

    private void ResetOnTouch()
    {
        viableJumps = playerStat.maxJumps;
    }

    public void ChangeState<T>() where T : IState
    {
        stateMachine.ChangeState<T>();
    }

    public void SetDirection(Vector2 vector2)
    {
        // Debug.Log($"Input data: {vector2}");
        MoveDirection = Mathf.Abs(vector2.x) >= 0.01f ? (int)Mathf.Sign(vector2.x) : 0;
        transform.localScale = new Vector3(MoveDirection != 0 ? MoveDirection : transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (MoveDirection != 0 && stateMachine.IsInState<PlayerIdleState>())
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

    public void SetDash()
    {
        stateMachine.ChangeState<PlayerDashState>();
    }

    // called in fixed update
    public void UpdateMoving()
    {
        transform.Translate(MoveDirection * playerStat.moveSpeed * Time.fixedDeltaTime * Vector2.right);
    }

    public async UniTask PerformDash()
    {
        float distanceTraveled = 0f;
        Vector2 startPosition = transform.position;
        var gravityScale = Rigidbody.gravityScale;
        Rigidbody.linearVelocity = Vector2.zero; // Reset velocity before dash
        Rigidbody.gravityScale = 0f; // Disable gravity during dash
        while (distanceTraveled < playerStat.dashRange)
        {
            float step = playerStat.dashSpeed * Time.fixedDeltaTime;
            transform.Translate(FacingDirection * step * Vector2.right);
            distanceTraveled = Vector2.Distance(startPosition, transform.position);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
        Rigidbody.gravityScale = gravityScale; // Restore gravity
        if (MoveDirection != 0)
            stateMachine.ChangeState<PlayerWalkingState>();
        else stateMachine.ChangeState<PlayerIdleState>();
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


    public void UnequipCurrentWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Unequip(this);
            currentWeapon = null;
        }
    }


    public void ChangeWeapon(PlayerWeaponData newWeapon)
    {
        Debug.Log($"Changing weapon to: {newWeapon.weaponName}");
        if (currentWeapon != null)
            currentWeapon.Unequip(this);

        if (newWeapon != null)
            newWeapon.Equip(this);
        currentWeapon = newWeapon;
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
        // CommandData selectedAttackCommand = attackCommands[0];
        var selectedAttackCommand = defaultAttackCommand;
        if (CurrentDirectionInput.y < -0.5f && attackCommands.Count > 1 && !isGrounded)
            selectedAttackCommand = pogoAttackCommand;
        // selectedAttackCommand = attackCommands[1];
        if (attackCommandInvoker.SetComboAttackData(selectedAttackCommand))
            attackCommandInvoker.ExecuteCommandsAsync().Forget();
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

    public void HandleSkillAttackInput()
    {
        if (skillAttackCommand != null)
        {
            attackCommandInvoker.SetComboAttackData(skillAttackCommand);
            attackCommandInvoker.ExecuteCommandsAsync().Forget();
        }
    }
}