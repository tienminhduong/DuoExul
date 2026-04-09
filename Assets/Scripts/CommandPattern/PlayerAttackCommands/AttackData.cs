using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/AttackData", fileName = "New AttackData")]
public class AttackData : ScriptableObject
{
    public string label;
    public AnimationData animationData;
    [SerializeReference] public List<IAbility> uniqueAbilities;
}