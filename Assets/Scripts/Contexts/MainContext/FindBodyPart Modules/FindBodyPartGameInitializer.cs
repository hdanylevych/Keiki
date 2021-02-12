using strange.extensions.mediation.impl;
using UnityEngine;

public class FindBodyPartGameInitializer : View
{
    [Inject] public FindBodyPartScenario FindBodyPartScenario { get; set; }

    private void Start()
    {
        base.Start();

        var unitViewInstance = new GameObject("UnitView");
        unitViewInstance.transform.parent = transform;

        var unitViewComponent = unitViewInstance.AddComponent<UnitView>();
        unitViewComponent.Initialize(FindBodyPartScenario.Model);
    }
}
