using UnityEngine;

public interface IAudioManager
{
    void Initialize(AudioSource sfxSource);
    void PlayOneShot(AudioClip clip);
}
