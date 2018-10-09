using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{


    public class MusicPlayer
    {
        private enum MusicChannelState
        {
            Stopped,
            Paused,
            Playing
        }

        private class MusicChannel
        {
            public AudioSource source;
            public MusicChannelState state;

            public MusicChannel(AudioSource a_source)
            {
                source = a_source;
                state = MusicChannelState.Stopped;
            }

            public void Init()
            {
                source.spatialBlend = 0f;
                source.playOnAwake = false;
            }

            public bool isPlaying()
            {
                return true;
            }
        }

        private AudioJobSystem _jobSystem;

        private MusicChannel[] _musicChannels;
        private int _currentChannel = 0;


        private AudioClip _currentMusicClip;

        public MusicPlayer(AudioSource a_musicSourceA, AudioSource a_musicSourceB, AudioJobSystem a_jobSystem)
        {
            _musicChannels = new MusicChannel[2];
            _musicChannels[0].source = a_musicSourceA;
            _musicChannels[1].source = a_musicSourceB;

            for(int i = 0; i < _musicChannels.Length; ++i)
            {
                _musicChannels[i].Init();
            }

            _jobSystem = a_jobSystem;
        }

        public bool isPlaying()
        {
            return _musicChannels[_currentChannel].isPlaying;
        }

        public bool isPlayingClip(AudioClip a_clip)
        {
            if (!_musicChannels[_currentChannel].isPlaying)
                return false;

            return _musicChannels[_currentChannel].clip == a_clip;
        }

        public void PlayMusicClip(AudioClip a_clip)
        {
            _currentMusicClip = a_clip;
            _musicChannels[_currentChannel].Pause();
            _musicChannels[_currentChannel].clip = _currentMusicClip;
            _musicChannels[_currentChannel].Play();
        }


        public void Play()
        {
            _musicChannels[_currentChannel].Play();
            VolumeLerpJob playJob = new VolumeLerpJob(_musicChannels[_currentChannel], 1f, 1f);
            _jobSystem.AddSyncJob(playJob);
        }

        public void Pause(AudioJobBase.AudioJobClbk onComplete = null)
        {
            VolumeLerpJob pauseJob = new VolumeLerpJob(_musicChannels[_currentChannel], 0f, 1f);
            pauseJob.onComplete += _musicChannels[_currentChannel].Pause;
            if(onComplete != null)
                pauseJob.onComplete += onComplete;

            _jobSystem.AddSyncJob(pauseJob);
        }

        public void Stop(bool immiediately = false)
        {
            _musicChannels[_currentChannel].Stop();
        }

        public void Reset()
        {
            Pause(InternalReset);
            _currentMusicClip = null;
        }

        private void InternalReset()
        {
            _musicChannels[_currentChannel].Stop();
            _currentMusicClip = null;
        }

    }
}