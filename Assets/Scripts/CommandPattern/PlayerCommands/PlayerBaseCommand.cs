using System.Threading.Tasks;
using UnityEngine;

public abstract class PlayerBaseCommand : ICommand
{
    public IEntity player;
    public PlayerBaseCommand(IEntity player)
    {
        this.player = player;
    }

    public abstract void Execute();
    public abstract Awaitable ExecuteAsync();
}