using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Command/CommandData", fileName = "New CommandData")]
public class CommandData : ScriptableObject
{
    public string label;
    [SerializeReference] public List<ICommand> commands;

    void OnEnable()
    {
        if (string.IsNullOrEmpty(label))
            label = name;

        commands ??= new List<ICommand>();
    }
}