using UnityEngine;

public interface IEntity
{
    Rigidbody2D Rigidbody { get; }
    int Direction { get; }

}