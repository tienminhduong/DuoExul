using UnityEngine;

public enum HitSourceType 
{
    // static hazard
    SPIKE,
    STALACTITE,
    GROUND_SPIKE,

    // mechanical traps
    SAWBLADE, // lưỡi cưa
    SPIKE_TRAP, // bẫy gai thò lên thụt xuống khi player đi qua

    // environment
    LAVA,
    POISON
}
