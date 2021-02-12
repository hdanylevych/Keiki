using strange.extensions.command.impl;

public class ProcessInteractionCommand : Command
{
    [Inject] public IInteractable Interactable { get; set; }
    [Inject] public BodyPartFoundSignal BodyPartFoundSignal { get; set; }

    public override void Execute()
    {
        Interactable.Interact();

        if (Interactable is IBodyPart bodyPart)
        {
            BodyPartFoundSignal.Dispatch(new BodyPartParameters()
                                             {
                                                 BodyPartType = bodyPart.BodyPartType,
                                                 ProperUnitType = bodyPart.ProperUnitType
                                             });
        }
    }
}
