using UnityEngine;
using UnityEngine.Serialization;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static bool HasInstance => _instance != null;
    public static T TryGetInstance() => HasInstance ? _instance : null;

    public static T Instance
    {
        get
        {
            if (_instance)
                return _instance;
            var gameObj = new GameObject(typeof(T).Name + " Auto-Generated");
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

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}