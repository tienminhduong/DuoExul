using UnityEngine;

public class DDTAbilityExecutor : MonoBehaviour
{
    [SerializeField] private DDTAbilityData abilityData;
    [SerializeField] private GameObject target;

    public void Execute(GameObject target)
    {
        foreach (var effect in abilityData.effects)
        {
            effect.Execute(gameObject, target);
        }
    }
}