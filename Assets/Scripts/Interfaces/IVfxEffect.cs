using UnityEngine;

public interface IVfxEffect
{
    void Play(Vector3 position, Quaternion rotation);
    void Stop();
    bool IsFinished { get; }
}
