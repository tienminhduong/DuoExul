using UnityEngine;

public interface IProjectile
{
    void Spawn(Vector2 pos, Quaternion rot, Vector2 direction);
}

public enum ProjectileType
{
    VirgoProjectile,
    AriesProjectile,
}