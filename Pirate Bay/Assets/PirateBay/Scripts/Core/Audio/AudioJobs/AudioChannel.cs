using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{
    public abstract class AudioChannel 
    {
        private enum AudioChannelState
        {
            Stopped,
            Paused,
            Playing
        }

        public AudioSource source;
        private AudioChannelState state;

        protected AudioChannel(AudioSource a_source)
        {
            source = a_source;
            state = AudioChannelState.Stopped;
        }

        public bool IsPlaying => state == AudioChannelState.Playing;

        public bool IsPaused => state == AudioChannelState.Paused;

        public bool IsStopped => state == AudioChannelState.Stopped;

        public bool IsPlayingClip(AudioClip a_clip)
        {
            if (state != AudioChannelState.Playing)
                return false;

            return source.clip == a_clip;
        }

        public AudioClip Clip
        {
            get { return source.clip; }
            set { source.clip = value; }
        }

        public float Volume
        {
            get => source.volume;
            set => source.volume = value;
        }

        public void Stop()
        {
            if (state == AudioChannelState.Stopped) return;

            source.Stop();
            state = AudioChannelState.Stopped;
        }

        public void Pause()
        {
            if (state == AudioChannelState.Paused) return;

            source.Pause();
            state = AudioChannelState.Paused;
        }

        public void Play()
        {
            if (state == AudioChannelState.Playing) return;

            source.Play();
            state = AudioChannelState.Playing;
        }
    }

    public class MusicChannel : AudioChannel
    {
        public MusicChannel(AudioSource a_source) : base (a_source)
        {
            source.spatialBlend = 0f;
            source.playOnAwake = false;
        }
    }

}

