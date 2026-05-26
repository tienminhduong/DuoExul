using UnityEngine;

public interface IDetectionTarget
{
    bool IsDetected();
    void SetActive(bool value);
}

public abstract class BaseDetectionTarget : MonoBehaviour, IDetectionTarget
{
    public abstract bool IsDetected();
    public abstract void SetActive(bool value);
    public abstract GameObject ObjectDetected { get; }
}