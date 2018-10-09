using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Audio;
using UnityEditor;


[RequireComponent(typeof(AudioJobSystem))]
public class AudioPlayer : SingletonBehaviour<AudioPlayer>
{
    private AudioJobSystem _audioJobSystem;

    private MusicPlayer _musicPlayer;
    [SerializeField]
    private AudioClipList _musicClipList;
    public AudioClip testClip;

    private void Awake()
    {
        _audioJobSystem = GetComponent<AudioJobSystem>();

        GameObject go = new GameObject("MusicSource", typeof(AudioSource), typeof (AudioSource));
        go.transform.SetParent(transform);
        AudioSource[] musicSources = go.GetComponents<AudioSource>();
        if (musicSources.Length == 2)
            _musicPlayer = new MusicPlayer(musicSources[0], musicSources[1], _audioJobSystem);
    }

    private const string GoblinMusic = "The Path of the Goblin King";

    [ContextMenu("StartPlayMusicTest")]
    private void StartPlayMusicTest()
    {
        AudioClip music = _musicClipList.GetClipByName(GoblinMusic);
        if (music != null)
        {
            _musicPlayer.PlayMusicClip(testClip);
        }
        else
        {
            Debug.LogError("No Music Found by that name!");
        }
            
    }

    [ContextMenu("PauseMusicTest")]
    private void PauseMusicTest()
    {
        _musicPlayer.Pause();
    }

    [ContextMenu("ResumeMusicTest")]
    private void ResumeMusicTest()
    {
        _musicPlayer.Play();
    }
}