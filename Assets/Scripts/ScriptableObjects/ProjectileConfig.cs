using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Scriptable Objects/ProjectileConfig")]
public class ProjectileConfig: ScriptableObject
{
    public ProjectileType type;
    public MonoBehaviour prefab;
    public int preloadCount;
}
