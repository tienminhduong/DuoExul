using UnityEngine;
using UnityEngine.Events;

public class AnimationController
{
    private Animator animator;
    public AnimationController(Animator animator)
    {
        this.animator = animator;
    }

    public UnityAction OnOverrideAnimationComplete;

    private async Awaitable CrossFade(AnimationData animationData, float transitionDuration = 0.1f)
    {
        Debug.Log($"Crossfading to state with hash: {animationData.Hash}");
        animator.CrossFade(animationData.Hash, transitionDuration);
        await Awaitable.WaitForSecondsAsync(transitionDuration + animationData.duration);
    }

    private AnimationData currentAnim;
    private AnimationData currentStandardAnim;
    public async Awaitable PlayAnimation(AnimationData anim, float transitionDuration = 0.1f)
    {
        if (anim.priority == AnimationData.PriorityLevel.Override)
        {
            if (currentAnim.priority == AnimationData.PriorityLevel.Standard)
                currentStandardAnim = currentAnim;

            currentAnim = anim;
            await CrossFade(anim, transitionDuration);
            OnOverrideAnimationComplete?.Invoke();
            await RevertStandardAnim(transitionDuration);
        }
        else
        {
            currentStandardAnim = anim;
            if (currentAnim.priority == AnimationData.PriorityLevel.Standard)
            {
                currentAnim = anim;
                await CrossFade(anim, transitionDuration);
            }
        }
    }

    private async Awaitable RevertStandardAnim(float transitionDuration = 0.1f)
    {
        currentAnim = currentStandardAnim;
        Debug.Log($"Reverting to standard animation: {currentStandardAnim.name}");
        await CrossFade(currentStandardAnim, transitionDuration);
    }
}

public struct AnimationData
{
    public enum PriorityLevel
    {
        Standard = 0,
        Override = 1,
    }
    public PriorityLevel priority;
    public string name;
    public float duration;

    public readonly int Hash => Animator.StringToHash(name);

    public AnimationData(PriorityLevel priority, string name, float duration)
    {
        this.priority = priority;
        this.name = name;
        this.duration = duration;
    }
}