using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))] // Đảm bảo luôn có Animator
public class BreakableFlatform : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] float timeShake = 5f;
    [SerializeField] float timeDisappear = 2f;

    [Header("Animation Names")]
    [SerializeField] string normalAnimation = "Normal";
    [SerializeField] string crackAnimation = "Cracking";
    [SerializeField] string disappearAnimation = "Disappear";

    private BoxCollider2D boxCollider;
    private Animator animator;
    [SerializeField] private bool isBreaking = false; // Thêm biến check để tránh trigger Coroutine nhiều lần

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            // Kiểm tra hướng va chạm (Player đứng trên đầu)
            if (collision.contacts[0].normal.y < -0.5f)
            {
                // QUAN TRỌNG: Khóa ngay tại đây trước khi bắt đầu Coroutine
                isBreaking = true;
                StartCoroutine(PlayerEnter());
            }
        }
    }

    private IEnumerator PlayerEnter()
    {
        // Không cần gán isBreaking = true ở đây nữa vì đã làm ở trên

        // 1. Giai đoạn rung/nứt
        animator.Play(crackAnimation);
        yield return new WaitForSeconds(timeShake);

        // 2. Giai đoạn biến mất
        boxCollider.enabled = false;
        animator.Play(disappearAnimation);
        yield return new WaitForSeconds(timeDisappear);

        // 3. Giai đoạn hồi phục
        animator.Play(normalAnimation);
        boxCollider.enabled = true;

        // Chờ thêm một khoảng ngắn để đảm bảo vật lý đã ổn định 
        // trước khi cho phép dẫm lên lần nữa
        yield return new WaitForSeconds(0.1f);
        isBreaking = false;
    }
}