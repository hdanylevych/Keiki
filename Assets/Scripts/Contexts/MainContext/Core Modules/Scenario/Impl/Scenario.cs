using System;

public abstract class Scenario
{
    protected bool _isActive;

    public bool IsActive => _isActive;

    public abstract string MiniGameLocation { get; }

    public event Action OnComplete;

    public abstract void Start();

    protected void InvokeOnComplete()
    {
        OnComplete?.Invoke();
    }
}