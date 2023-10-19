using System;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key)
    {
        StateKey = key;
    }
    
    public EState StateKey { get; private set; }
    protected EState _nextStateKey;
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
    public abstract EState GetNextState();
}
