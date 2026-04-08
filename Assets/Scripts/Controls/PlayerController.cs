using System.Collections.Generic;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour, IEntity
{
    public Rigidbody2D Rigidbody { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public int Direction { get; private set; }
    public AnimationController AnimationController { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private int maxJumps = 1;

    [SerializeField] private LayerMask groundLayer;

    [Header("Reference")]
    [SerializeField] private GameObject hitbox;
    [SerializeField] private ChainCommandInvoker attackCommandInvoker;


    [Header("Debug readonly")]
    [ReadOnly][SerializeField] private StateMachine stateMachine;

    // For double jumping, reset if touch the ground, -1 when leave the ground
    [ReadOnly][SerializeField] private int viableJumps = 1;


    // Actions
    public UnityAction OnGrounded;


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        AnimationController = new AnimationController(GetComponentInChildren<Animator>());

        SetupStateMachine();
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
        Direction = vector2.x != 0 ? (int)Mathf.Sign(vector2.x) : 0;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        ResetOnTouch();

        if (groundLayer.Contains(collision.gameObject))
            OnGrounded?.Invoke();
    }

    public void Attack(AttackData attackData)
    {
        hitbox.SetActive(true);
    }

    public void HandleAttackInput()
    {
        var attackCommand = attackCommandInvoker.ExecuteCommandsAsync();
    }

    public void HandleAttackInputCancel()
    {
        attackCommandInvoker.PauseExecution();
    }
}