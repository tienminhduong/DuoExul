using UnityEngine;

public interface IDetectionTarget
{
    void OnEnterRange(PlayerController player);
    void OnStayRange(PlayerController player);
    void OnExitRange(PlayerController player);
}