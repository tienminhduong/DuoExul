public abstract class PlayerBaseCommand : ICommand
{
    IEntity player;
    public PlayerBaseCommand(IEntity player)
    {
        this.player = player;
    }

    public abstract void Execute();
}