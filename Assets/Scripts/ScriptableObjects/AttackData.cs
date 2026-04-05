using UnityEngine;

[CreateAssetMenu(menuName = "Data/AttackData", fileName = "New AttackData")]
public class AttackData : ScriptableObject
{
    public string label;
    public AnimationData animationData;
    public int baseDamage = 10;
}