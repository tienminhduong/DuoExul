using UnityEngine;

public class AriesFireEffect : MonoBehaviour, IVfxEffect
{
    [SerializeField] private VfxType type;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("AriesFireEffect requires an Animator component.");
            enabled = false;
        }
    }
    public bool IsFinished => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;

    public void Play(Vector3 position, Quaternion rotation)
    {
        this.transform.SetPositionAndRotation(position, rotation);
        gameObject.SetActive(true);
        animator.Play("Fire");
    }

    public void Stop()
    {
        animator.StopPlayback();
    }

    public void OnAnimationComplete()
    {
        VFXSystem.Instance?.ReturnPool(this.type, this);
    }
}
