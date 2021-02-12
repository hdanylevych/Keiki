using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scenario
{
    protected bool _isActive;
    
    public abstract string MiniGameLocation { get; }
    public bool IsActive => _isActive;

    public event Action OnComplete;

    public abstract void Start();

    protected void InvokeOnComplete()
    {
        OnComplete?.Invoke();
    }
}
