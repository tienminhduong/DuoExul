using System;
using System.Collections.Generic;

public class StateMachine
{
    // StateNode currentState;
    IState currentState;
    readonly Dictionary<Type, IState> states = new();

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void SetState<T>(T state) where T : IState
    {
        currentState = states[state.GetType()];
        currentState?.Enter();
    }

    public void ChangeState<T>() where T : IState
    {
        currentState?.Exit();
        currentState = states[typeof(T)];
        currentState?.Enter();
    }

    public void AddState(IState state)
    {
        states[state.GetType()] = state;
    }
}
