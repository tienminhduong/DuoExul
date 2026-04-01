using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

[Serializable]
public class StateMachine
{
    // StateNode currentState;
    [ReadOnly] [SerializeReference] IState currentState;
    readonly Dictionary<Type, IState> states = new();

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void SetState<T>() where T : IState
    {
        currentState = states[typeof(T)];
        currentState?.Enter();
    }

    public void ChangeState<T>() where T : IState
    {
        currentState?.Exit();
        currentState = states[typeof(T)];
        currentState?.Enter();
    }

    public bool IsInState<T>() where T : IState
    {
        return currentState is T;
    } 

    public void AddState(IState state)
    {
        states[state.GetType()] = state;
    }
}
