using System;

public interface IUnitModel
{
    BodyPartType MissingBodyPartType { get; }
    UnitType Type { get; }
    UnitState State { get; }

    event Action OnTypeChanged;
}
