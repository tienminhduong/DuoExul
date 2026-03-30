using UnityEngine;

public interface IPogoable
{
    bool CanPogo();
    void OnPogo(PlayerController player);
}