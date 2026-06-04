using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ZombieHurt: IState
{
    Zombie zombie;
    Animator animator;

    public ZombieHurt(Zombie zombie, Animator animator)
    {
        this.zombie = zombie;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("[ZombieHurt] Enter");
        // Chạy animation Hurt
        animator.Play(Zombie.HURT_ANIMATION);
    }

    public void Exit()
    {
        
    }

    public void FixedUpdate()
    {
        
    }

    public bool IsFinished()
    {
        // Lấy thông tin về state hiện tại của layer đang chạy
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Kiểm tra xem Animator có đang chuyển cảnh (transition) không
        if (animator.IsInTransition(0))
        {
            // Nếu đang chuyển cảnh TỪ state cần check sang state khác, coi như nó đã kết thúc
            return !stateInfo.IsName(Zombie.HURT_ANIMATION);
        }

        // Kiểm tra đúng tên state và normalizedTime >= 1.0f (1.0f nghĩa là đã chạy xong 100%)
        return stateInfo.IsName(Zombie.HURT_ANIMATION) && stateInfo.normalizedTime >= 1f;
    }

    public void Update()
    {
        
    }
}
