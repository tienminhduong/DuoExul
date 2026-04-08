using UnityEngine;

public interface IPogoable
{
    bool CanPogo(GameObject source); // Check if can pogo
    void OnPogo(); // do anything by itself
}