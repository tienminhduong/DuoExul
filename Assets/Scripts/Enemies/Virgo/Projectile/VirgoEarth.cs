using UnityEngine;

public class VirgoEarth : MonoBehaviour, IProjectile
{
    const string ANIMATION_EARTH = "VirgoEarth";
    Animator _animator;

    [SerializeField] private ProjectileType _type;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Spawn(Vector2 pos, Quaternion rot, Vector2 direction)
    {
        this.transform.position = pos;
        this.transform.rotation = rot;
        _animator.Play(ANIMATION_EARTH);
    }

    public void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ProjectileSystem.Instance.Return(_type, this);
        }
    }
}
