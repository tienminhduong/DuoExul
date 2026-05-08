using AIEnemy;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(HealthComponent))]
public class AriesController : BaseEnemyController, IEntity
{
    [SerializeField] PlayerDetector playerDetector;

    [Header("Skill2")]
    [SerializeField] int numberOfFireColumnsInPunchGroundSkill = 3;
    [SerializeField] float distanceOfTwoColumns = 5;
    [SerializeField] Vector2 offsetFirstColumnFromAriesCenter = new Vector2(1, 0);
    [SerializeField] Vector2 offsetAxisY = new Vector2(0, 1);

    Animator animator;
    AIStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stateMachine = new AIStateMachine();

        var idleState = new IdleAriesState(this, animator, "AriesIdle");
        var moveState = new MoveAriesState(this, animator, "AriesRun", playerDetector.Player);
        var hurtState = new HurtAriesState(this, animator, "AriesHurt");
        var dieState = new DieAriesState(this, animator, "AriesDie");
        var punchGroundSkillState = new PunchGroudSkillState(this, animator, "AriesPunchGround");

        At(idleState, moveState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(idleState, hurtState, new FuncPredicate(() => IsHurt()));
        At(moveState, idleState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(moveState, hurtState, new FuncPredicate(() => IsHurt()));
        At(hurtState, idleState, new FuncPredicate(() => hurtState.IsFinished()));
        At(hurtState, dieState, new FuncPredicate(() => HealthComponent.CurrentHealth <= 0));
        At(moveState, punchGroundSkillState, new FuncPredicate(() => playerDetector.CanDetectPlayer() && Random.value < 0.01f)); // 1% chance mỗi frame khi phát hiện player
        At(punchGroundSkillState, idleState, new FuncPredicate(() => punchGroundSkillState.IsFinished()));
        At(punchGroundSkillState, hurtState, new FuncPredicate(() => IsHurt()));

        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Debug.Log("Aries hit by player attack");
            SetHurt(true);
        }
    }

    public override void SetDie()
    {
        base.SetDie();
        _collider.enabled = false;
        _rigidbody.gravityScale = 0;
    }

    public IEnumerator MakeFireClolumn()
    {
        Vector2 effectPosition = (Vector2)transform.position + offsetFirstColumnFromAriesCenter + offsetAxisY; // Khởi tạo vị trí ban đầu là vị trí của Aries
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Xác định hướng dựa trên hướng của Aries
        for(int i = 0; i < numberOfFireColumnsInPunchGroundSkill; i++)
        {
            effectPosition += (direction * distanceOfTwoColumns); // Di chuyển vị trí cho cột lửa tiếp theo
            VFXSystem.Instance?.Play(VfxType.AriesFire, effectPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.2f); // Đợi một khoảng thời gian trước khi tạo cột lửa tiếp theo
        }
    }    
}
