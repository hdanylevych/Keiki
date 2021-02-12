using System;

public class UnitModel : IUnitModel
{
    private UnitState _state;
    private BodyPartType _missingBodyPartType;
    private UnitType _type;

    public BodyPartType MissingBodyPartType => _missingBodyPartType;
    
    public UnitType Type
    {
        get => _type;

        set
        {
            if (_type == value)
                return;
            
            _type = value;

            OnTypeChanged?.Invoke();
        }
    }

    public UnitState State
    {
        get => _state;

        set
        {
            _state = value;
        }
    }
    
    public event Action OnTypeChanged;
    
    public UnitModel(UnitType type, BodyPartType missingBodyPartType)
    {
        _type = type;
        _missingBodyPartType = missingBodyPartType;
    }
}

public enum UnitState
{
    Idle = 0,
    Sad = 1
}
