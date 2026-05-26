using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    UniTask Execute(IEntity entity);
}