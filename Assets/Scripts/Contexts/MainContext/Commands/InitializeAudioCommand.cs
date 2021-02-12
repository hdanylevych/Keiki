
using strange.extensions.command.impl;
using strange.extensions.context.api;

using UnityEngine;

public class InitializeAudioCommand : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject Root { get; set; }
    [Inject] public IAudioManager AudioManager { get; set; }

    public override void Execute()
    {
        var audioSourceInstance = new GameObject("GameAudioSource");
        
        var sfxSourceComponent = audioSourceInstance.AddComponent<AudioSource>();
        sfxSourceComponent.playOnAwake = false;
        sfxSourceComponent.priority = 30;

        AudioManager.Initialize(sfxSourceComponent);
    }
}
