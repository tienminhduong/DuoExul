using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSystem : MonoBehaviour
{
    public static VFXSystem Instance { get; private set; }

    [SerializeField] private List<VfxConfig> configs;

    private Dictionary<VfxType, ObjectPool<MonoBehaviour>> pools;

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
        pools = new Dictionary<VfxType, ObjectPool<MonoBehaviour>>();

        foreach (var config in configs)
        {
            var pool = new ObjectPool<MonoBehaviour>(config.prefab, config.preloadCount, transform);
            pools.Add(config.type, pool);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Playing CutGrass VFX");
            Play(VfxType.CutGrass, this.transform.position, Quaternion.identity);
            Debug.Log("CutGrass VFX played");
        }
    }

    public void Play(VfxType type, Vector2 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning($"VFX type {type} not found!");
            return;
        }

        var obj = pool.Get();
        var effect = obj as IVfxEffect;

        if (effect == null)
        {
            Debug.LogError("VFX prefab does not implement IVfxEffect");
            return;
        }

        effect.Play(pos, rot);

        //StartCoroutine(ReturnWhenDone(effect, obj, pool));
    }

    private IEnumerator ReturnWhenDone(IVfxEffect effect, MonoBehaviour obj, ObjectPool<MonoBehaviour> pool)
    {
        while (!effect.IsFinished)
            yield return null;

        pool.Return(obj);
    }

    public void ReturnPool(VfxType type, MonoBehaviour obj)
    {
        if (pools.TryGetValue(type, out var pool))
        {
            pool.Return(obj);
        }
        else
        {
            Debug.LogWarning($"VFX type {type} not found for returning!");
        }
    }
}
