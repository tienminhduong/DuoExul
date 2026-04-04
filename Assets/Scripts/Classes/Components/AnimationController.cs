using UnityEngine;

public class AnimationController
{
    public static readonly int IdleHash = Animator.StringToHash("Idle");
    public static readonly int WalkHash = Animator.StringToHash("Walk");
    public static readonly int RunHash = Animator.StringToHash("Run");
    public static readonly int AttackHash = Animator.StringToHash("Attack");
    public static readonly int JumpHash = Animator.StringToHash("Jump");
    public static readonly int FallHash = Animator.StringToHash("Fall");

    private Animator animator;
    public AnimationController(Animator animator)
    {
        this.animator = animator;
    }

    public void CrossFade(int stateHash, float transitionDuration = 0.1f)
    {
        Debug.Log($"Crossfading to state with hash: {stateHash}");
        animator.CrossFade(stateHash, transitionDuration);
    }

    public void CrossFade(string stateName, float transitionDuration = 0.1f)
    {
        animator.CrossFade(stateName, transitionDuration);
    }
}