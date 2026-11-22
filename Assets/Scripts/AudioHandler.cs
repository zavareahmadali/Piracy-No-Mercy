using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class AudioHandler : MonoBehaviour
{
    private Dictionary<SFXType, AudioSource> audioSources;

    [Header("Sound Clips")]
    [SerializeField] private Dictionary<SFXType, AudioClip[]> audioClips;

    private void Start()
    {
        InitializeAudioSources();
    }
    public void PlaySFX(SFXType type)
    {
        InitializeAudioSources();
        if (!audioClips.TryGetValue(type, out AudioClip[] clips) || clips.Length == 0)
            return;

        if (!audioSources.ContainsKey(type))
            audioSources.Add(type, gameObject.AddComponent<AudioSource>());
        audioSources[type].pitch = UnityEngine.Random.Range(0.7f, 1.5f);
        audioSources[type].PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    private void InitializeAudioSources()
    {
        if (audioSources == null)
        {
            audioSources = new Dictionary<SFXType, AudioSource>();
        }
    }
}

public enum SFXType
{
    Shoot,
    Hit,
    Loot,
    Boost,
    Explosion,
    Select,
    Cancel,
    Open,
    Close,
    Bought,
    Brush, 
    LevelUp
}

