using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

public class InitializeScenarioCommand : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject Root { get; set; }
    [Inject] public IScenarioController ScenarioController { get; set; }

    public override void Execute()
    {
        ScenarioController.CurrentScenario.Start();

        var miniGamePrefab = Resources.Load<GameObject>(ScenarioController.CurrentScenario.MiniGameLocation);

        if (miniGamePrefab == null)
        {
            Debug.LogError($"InitializeScenarioCommand: could not load game by location: {ScenarioController.CurrentScenario.MiniGameLocation}.");
        }

        GameObject.Instantiate(miniGamePrefab, Root.transform);
    }
}
