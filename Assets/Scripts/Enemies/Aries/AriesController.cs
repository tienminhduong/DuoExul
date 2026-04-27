using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(HealthComponent))]
public class AriesController : BaseEnemyController
{
    [SerializeField] private BaseDetectionTarget playerDetector;
    [SerializeField] private List<LayerMask> hurtLayers;

    private BoxCollider2D _boxCollider;
    protected override void Awake()
    {
        base.Awake();
        _boxCollider = GetComponent<BoxCollider2D>();
        _healthComponent.OnDeath += Die;
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        _stateMachine = new StateMachine();
        _stateMachine.AddState(new IdleAriesState(_animationController, this));
        _stateMachine.AddState(new MoveAriesState(_animationController, this));
        _stateMachine.AddState(new HurtAriesState(_animationController, this));
        _stateMachine.AddState(new DieAriesState(_animationController, this));
        _stateMachine.ChangeState<IdleAriesState>();
    }

    private void Update()
    {
        _stateMachine?.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }
    public override bool CanSeePlayer()
    {
        return playerDetector.IsDetected();
    }

    public override GameObject GetPlayer()
    {
        return playerDetector.ObjectDetected;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDead)
            return;

        if (!_boxCollider.IsTouching(collision))
            return;

        foreach (var layer in hurtLayers)
        {
            if (((1 << collision.gameObject.layer) & layer) != 0)
            {
                _stateMachine?.ChangeState<HurtAriesState>();
            }
        }
    }

    protected override void Die()
    {
        if (isDead)
            return;
        isDead = true;
        _boxCollider.enabled = false;
        _rigidbody.gravityScale = 0;
        playerDetector.SetActive(false);
        ChangeState<DieAriesState>();
    }
}
