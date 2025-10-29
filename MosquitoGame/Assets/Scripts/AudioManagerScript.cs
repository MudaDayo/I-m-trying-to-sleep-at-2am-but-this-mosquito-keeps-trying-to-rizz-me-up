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
    public static void PlayMusic(int audioSource, int listIndex, float volume, float pitch)
    {
        if (_instance == null) return;
        _instance.allAudioSources[audioSource].volume = volume;
        _instance.allAudioSources[audioSource].pitch = pitch;
        _instance.allAudioSources[audioSource].clip = _instance.sounds[listIndex];
        _instance.allAudioSources[audioSource].loop = true;
        _instance.allAudioSources[audioSource].Play();
    }
    public static void StopMusic(int audioSource)
    {
        if (_instance == null) return;
        _instance.allAudioSources[audioSource].Stop();
    }
    public static void PauzeMusic(int audioSource)
    {
        if (_instance == null) return;
        _instance.allAudioSources[audioSource].Pause();
    }
    public static void ResumeMusic(int audioSource)
    {
        if (_instance == null) return;
        _instance.allAudioSources[audioSource].UnPause();
    }
}
