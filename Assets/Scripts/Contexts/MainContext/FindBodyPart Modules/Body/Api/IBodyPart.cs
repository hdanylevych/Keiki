public interface IBodyPart : IInteractable
{
    BodyPartType BodyPartType { get; }
    UnitType ProperUnitType { get; }
}
