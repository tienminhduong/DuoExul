using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static bool HasInstance => _instance != null;
    public static T TryGetInstance() => HasInstance ? _instance : null;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;

            GameObject gameObj = new($"{typeof(T).Name} Auto-Generated");
            _instance = gameObj.AddComponent<T>();
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        if (!Application.isPlaying) return;

        _instance = this as T;
    }
}
