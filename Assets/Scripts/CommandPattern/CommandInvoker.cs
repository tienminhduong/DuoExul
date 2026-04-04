using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    [SerializeField] public CommandData commandData;
    [SerializeField] private GameObject target;
    public void ExecuteCommand()
    {
        Debug.Log($"Executing command: {commandData.label}");
        foreach (var command in commandData.commands)
        {
            command.Execute();
        }
    }

    public async Awaitable ExecuteCommandAsync()
    {
        Debug.Log($"Executing command: {commandData.label}");
        foreach (var command in commandData.commands)
        {
            await command.ExecuteAsync();
        }
        

        if (commandData.commands.Count > 0 && commandData.commands[0] is PlayerAttackCommand pc)
        {
            if (pc.player is PlayerController pcc)
            {
                pcc.ChangeState<PlayerIdleState>();
            }
        }
    }
}