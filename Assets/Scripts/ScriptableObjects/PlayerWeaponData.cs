using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Weapon", menuName = "ScriptableObjects/PlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    public string weaponName;
    [SerializeReference] private List<IOvertimeEffect> ApplyingEffects;
    public CommandData AttackCommand;

    public void Equip(PlayerController player)
    {
        player.currentWeapon = this;
        foreach (var effect in ApplyingEffects)
        {
            effect.ApplyEffect(player.playerStat);
        }
    }

    public void Unequip(PlayerController player)
    {
        if (player.currentWeapon != this)
            return;

        foreach (var effect in ApplyingEffects)
        {
            effect.RemoveEffect(player.playerStat);
        }
    }
}