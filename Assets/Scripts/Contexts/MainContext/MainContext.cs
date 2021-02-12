using System;
using System.Linq;

using strange.extensions.context.impl;
using strange.extensions.signal.impl;

using UnityEngine;

public class MainContext : MVCSContext
{
    public MainContext() : base()
    {
    }

    public MainContext(MonoBehaviour view, bool autoStartup)
        : base(view, autoStartup)
    {
    }

    protected override void mapBindings()
    {
        AutoBindType<BaseSignal>();
        AutoBindType<Scenario>();

        injectionBinder.Bind<InputProcessor>()
            .To<InputProcessor>()
            .ToSingleton();

        injectionBinder.Bind<IAudioManager>()
            .To<AudioManager>()
            .ToSingleton();

        injectionBinder.Bind<IUnityUpdateController>()
            .To<UnityUpdateController>()
            .ToSingleton();

        injectionBinder.Bind<IScenarioController>()
            .To<ScenarioController>()
            .ToSingleton();
        
        commandBinder.Bind<InitiateInteractionSignal>()
            .To<ProcessInteractionCommand>();

        commandBinder.Bind<ContextStartSignal>()
            .To<SetScreenOrientationSettingsCommand>()
            .To<InitializeUpdateLoopCommand>()
            .To<InitializeInputProcessorCommand>()
            .To<InitializeAudioCommand>()
            .To<InitializeScenarioCommand>()
            .InSequence()
            .Once();
    }

    private void AutoBindType<T>()
    {
        var scenariosTypes = GetType().Assembly.GetTypes().Where((type) => type.IsSubclassOf(typeof(T)));

        foreach (var scenarioType in scenariosTypes)
        {
            try
            {
                injectionBinder.Bind(scenarioType).ToSingleton();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
