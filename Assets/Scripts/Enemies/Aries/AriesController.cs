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

        At(idleState, moveState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(idleState, hurtState, new FuncPredicate(() => IsHurt()));
        At(moveState, idleState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(moveState, hurtState, new FuncPredicate(() => IsHurt()));
        At(hurtState, idleState, new FuncPredicate(() => hurtState.IsFinished()));
        At(hurtState, dieState, new FuncPredicate(() => HealthComponent.CurrentHealth <= 0));

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
}
