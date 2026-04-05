using UnityEngine;

[RequireComponent(typeof(IEntity))]
public class CommandInvoker : MonoBehaviour
{
    [SerializeField] private CommandData commandData;
    private IEntity entity;

    void Awake()
    {
        entity = GetComponent<IEntity>();
    }

    public async Awaitable ExecuteCommandAsync()
    {
        foreach (var command in commandData.commands)
        {
            await command.Execute(entity);
        }
    }
}