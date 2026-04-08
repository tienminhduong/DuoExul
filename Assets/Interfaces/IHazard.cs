using UnityEngine;

public interface IHazard
{
    int Damage { get; }
    bool IsActive { get; }
}