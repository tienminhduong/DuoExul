using UnityEngine;

[CreateAssetMenu(fileName = "VfxConfig", menuName = "Scriptable Objects/VfxConfig")]
public class VfxConfig : ScriptableObject
{
    public VfxType type;
    public MonoBehaviour prefab;
    public int preloadCount = 5;
}
