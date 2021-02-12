using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

public class AudioManager : IAudioManager
{
    private const string CorrectAnswerClipsLocation = "Sounds/Voices/Mother/CA";
    private const string IncorrectAnswerClipsLocation = "Sounds/Voices/Mother/IA";

    private bool _initialized = false;
    private AudioSource _sfxSource;
    private List<AudioClip> _correctAnswerClips;
    private List<AudioClip> _incorrectAnswerClips;
    private Queue<AudioClip> _playingQueue = new Queue<AudioClip>(5);

    public bool Initialized => _initialized;

    [Inject] public CorrectAnswerSignal CorrectAnswerSignal { get; set; }
    [Inject] public IncorrectAnswerSignal IncorrectAnswerSignal { get; set; }
    [Inject] public IUnityUpdateController UnityUpdateController { get; set; }

    public void Initialize(AudioSource sfxSource)
    {
        if (Initialized)
            return;

        UnityUpdateController.UpdateAgent.OnFixedUpdate += Update;

        _sfxSource = sfxSource;

        _correctAnswerClips = Resources.LoadAll<AudioClip>(CorrectAnswerClipsLocation).ToList();
        _incorrectAnswerClips = Resources.LoadAll<AudioClip>(IncorrectAnswerClipsLocation).ToList();

        CorrectAnswerSignal.AddListener(() => PlayRandomClipFromList(_correctAnswerClips));
        IncorrectAnswerSignal.AddListener(() => PlayRandomClipFromList(_incorrectAnswerClips));

        _initialized = true;
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (_sfxSource.isPlaying)
        {
            _playingQueue.Enqueue(clip);
            return;
        }

        _sfxSource.PlayOneShot(clip);
    }

    private void PlayRandomClipFromList(List<AudioClip> clips)
    {
        int randomIndex = Random.Range(0, clips.Count - 1);
        var clip = clips[randomIndex];

        if (_sfxSource.isPlaying)
        {
            _playingQueue.Enqueue(clip);
            return;
        }

        PlayOneShot(clip);
    }

    private void Update(float deltaTime)
    {
        if (_sfxSource.isPlaying == false && _playingQueue.Count > 0)
        {
            PlayOneShot(_playingQueue.Dequeue());
        }
    }

    private void StopAllClips()
    {
        _sfxSource.Stop();
    }
}
