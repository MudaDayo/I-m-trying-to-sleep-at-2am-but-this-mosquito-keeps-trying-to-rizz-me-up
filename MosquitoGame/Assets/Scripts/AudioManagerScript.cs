using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    private static AudioManagerScript _instance;
    public List<AudioSource> allAudioSources;

    public List<AudioClip> sounds;

    private void Start()
    {
        if (_instance == null)
            _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public static void PlaySound(int audioSource, int listIndex, float volume, float pitch)
    {
        if (_instance == null) return;
        _instance.allAudioSources[audioSource].volume = volume;
        _instance.allAudioSources[audioSource].pitch = pitch;
        _instance.allAudioSources[audioSource].PlayOneShot(_instance.sounds[listIndex]);
    }
}
