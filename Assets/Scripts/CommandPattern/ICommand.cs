using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    Awaitable Execute(IEntity entity);
}