using UnityEngine;

public class ZombieWalk : IState
{
    Zombie zombie;
    Animator animator;

    public ZombieWalk(Zombie zombie, Animator animator)
    {
        this.zombie = zombie;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("[ZombieWalk] Enter");
        animator.Play(Zombie.WALK_ANIMATION);
    }

    public void Exit()
    {
        
    }

    public void FixedUpdate()
    {
        // gọi hàm move của zombie để di chuyển về phía player
    }

    public bool IsFinished()
    {
        return false;
    }

    public void Update()
    {
        
    }
}
