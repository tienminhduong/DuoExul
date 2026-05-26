using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public static ProjectileSystem Instance { get; private set; }

    [SerializeField] public List<ProjectileConfig> configs;

    private Dictionary<ProjectileType, ObjectPool<MonoBehaviour>> pools;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        PrepareObject();
    }

    private void PrepareObject()
    {
        pools = new Dictionary<ProjectileType, ObjectPool<MonoBehaviour>>();
        foreach (var config in configs)
        {
            var pool = new ObjectPool<MonoBehaviour>(config.prefab, config.preloadCount, transform);
            pools.Add(config.type, pool);
        }
    }

    public void Spawn(ProjectileType type, Vector2 pos, Quaternion rot, Vector2 direction)
    {
        if (!pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"Projectile type {type} not found!");
            return;
        }
        var obj = pool.Get();
        var projectile = obj as IProjectile;
        if (projectile == null)
        {
            Debug.LogError("Projectile prefab does not implement IProjectile");
            return;
        }
        projectile.Spawn(pos, rot, direction);
    }

    public void Return(ProjectileType type, MonoBehaviour obj)
    {
        if (!pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"Projectile type {type} not found!");
            return;
        }
        pool.Return(obj);
    }
}
