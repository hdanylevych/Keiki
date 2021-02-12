using strange.extensions.command.impl;

public class InitializeInputProcessorCommand : Command
{
    [Inject] public InputProcessor InputProcessor { get; set; }

    public override void Execute()
    {
        InputProcessor.Initialize();
    }
}
