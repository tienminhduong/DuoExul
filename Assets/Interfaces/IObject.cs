public interface IObject : IEntity
{
    ObjectType ObjectType { get; }
}

public enum ObjectType
{
    HardObject,
    SoftObject
}