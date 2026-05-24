using System.Collections.Generic;
using UnityEngine;

public class VirgoProjectile : MonoBehaviour, IEntity, IProjectile
{
    const string ANIMATION_FLY = "VirgoProjectileFly";
    const string ANIMATION_EXPLODE = "VirgoProjectileExplode";

    [SerializeField] private float _speed = 15;
    [SerializeField] private ProjectileType _type;
    [SerializeField] private float _timeToLive = 5f;
    Rigidbody2D _rigidbody2d;
    public Rigidbody2D Rigidbody => _rigidbody2d;

    Animator _animator;
    float _remainTime;

    public void Spawn(Vector2 pos, Quaternion rot, Vector2 direction)
    {
        _animator.Play(ANIMATION_FLY);
        this.transform.position = pos;
        this.transform.rotation = rot;
        _rigidbody2d.linearVelocity = direction.normalized * _speed; // Adjust speed as needed
        _remainTime = _timeToLive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileSystem.Instance.Return(_type, this);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(_remainTime > 0)
        {
            _remainTime -= Time.deltaTime;
            if (_remainTime <= 0)
            {
                ProjectileSystem.Instance.Return(_type, this);
            }
        }
    }
}
