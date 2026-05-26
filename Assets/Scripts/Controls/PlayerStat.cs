using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public float moveSpeed = 5f;
    public float jumpHeight = 6f;
    public int maxJumps = 1;
    public float baseAttack = 10;
    public float dashSpeed = 20f;
    public float dashRange = 5f;

    public List<IOvertimeEffect> OvertimeEffects { get; private set; }

    public PlayerStat()
    {
        OvertimeEffects = new List<IOvertimeEffect>();
    }
}