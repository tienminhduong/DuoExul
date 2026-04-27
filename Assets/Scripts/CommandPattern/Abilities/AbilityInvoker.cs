using System.Collections.Generic;

public class AbilityInvoker
{
    public static void ApplyEffect(List<IAbility> abilities, IEntity executor, IEntity target)
    {
        foreach (var ability in abilities)
        {
            ability.ApplyEffect(executor, target);
        }
    }
}