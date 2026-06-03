using UnityEngine;

public class ZombieIdle : IState
{
    Zombie zombie;
    Animator animator;

    public ZombieIdle(Zombie zombie, Animator animator)
    {
        this.zombie = zombie;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("[ZombieIdle] Enter");
        // Chạy animation Idle
        animator.Play(Zombie.IDLE_ANIMATION);
    }

    public void Exit()
    {
        
    }

    public void FixedUpdate()
    {
    }

    public bool IsFinished()
    {
        return false;
    }

    public void Update()
    {
    }
}
