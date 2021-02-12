using strange.extensions.injector.api;
using System.Collections.Generic;
using System.Linq;

public class ScenarioController : IScenarioController
{
    private Queue<Scenario> _scenarios;
    private Scenario currentScenario;

    [Inject] public IInjectionBinder InjectionBinder { get; set; }

    public Scenario CurrentScenario => currentScenario;

    [PostConstruct]
    private void Construct()
    {
        LoadScenarios();

        currentScenario = _scenarios.Dequeue();
    }

    private void LoadScenarios()
    {
        var scenariosTypes = GetType().Assembly.GetTypes().Where((type) => type.IsSubclassOf(typeof(Scenario)));
        
        _scenarios = new Queue<Scenario>(scenariosTypes.Count());
        
        foreach (var scenarioType in scenariosTypes)
        {
            Scenario scenarioInstance = InjectionBinder.GetInstance(scenarioType) as Scenario;
            
            _scenarios.Enqueue(scenarioInstance);
        }
    }
}
