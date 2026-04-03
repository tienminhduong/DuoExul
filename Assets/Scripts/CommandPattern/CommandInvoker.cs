using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    [SerializeField] private CommandData commandData;
    [SerializeField] private GameObject target;
    public void ExecuteCommand(List<ICommand> commands)
    {
        foreach (var command in commands)
        {
            command.Execute();
        }
    }
}