using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    void Execute();
    Awaitable ExecuteAsync();
}