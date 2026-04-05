using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    [SerializeField] public CommandData commandData;
    [SerializeField] private GameObject target;
    public void ExecuteCommand()
    {
        foreach (var command in commandData.commands)
        {
            command.Execute();
        }
    }

    public async Awaitable ExecuteCommandAsync()
    {
        foreach (var command in commandData.commands)
        {
            await command.ExecuteAsync();
        }
    }
}