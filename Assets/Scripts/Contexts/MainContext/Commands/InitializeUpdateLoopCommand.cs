using strange.extensions.command.impl;

using strange.extensions.context.api;

using UnityEngine;

public class InitializeUpdateLoopCommand : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject ContextRoot { get; set; }
    [Inject] public IUnityUpdateController UnityUpdateController { get; set; }

    public override void Execute()
    {
        var updateAgent = UnityUpdateAgent.Create(ContextRoot);

        UnityUpdateController.Initialize(updateAgent);
    }
}
