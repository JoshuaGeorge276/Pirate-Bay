using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{


    public class MusicPlayer
    {
        private AudioJobSystem _jobSystem;

        private MusicChannel[] _musicChannels;
        private bool _currentChannel = false;

        public int CurrentChannel => _currentChannel ? 1 : 0;

        public void SwitchChannel()
        {
            _currentChannel = !_currentChannel;
        }

        public MusicPlayer(AudioSource a_musicSourceA, AudioSource a_musicSourceB, AudioJobSystem a_jobSystem)
        {
            _musicChannels = new MusicChannel[2];

            MusicChannel a = new MusicChannel(a_musicSourceA);
            MusicChannel b = new MusicChannel(a_musicSourceB);

            _musicChannels[0] = a;
            _musicChannels[1] = b;

            _jobSystem = a_jobSystem;
        }

        public bool IsPlaying()
        {
            return _musicChannels[CurrentChannel].IsPlaying;
        }

        public bool IsPlayingClip(AudioClip a_clip)
        {
            return _musicChannels[CurrentChannel].IsPlayingClip(a_clip);
        }

        public void PlayMusicClip(AudioClip a_clip, float a_overTime = 1f)
        {
            if (_musicChannels[CurrentChannel].IsPlayingClip(a_clip))
            {
                Debug.Log("MusicPlayer: Already Playing Clip!");
                return;
            }

            if(_musicChannels[CurrentChannel].IsPlaying)
                _musicChannels[CurrentChannel].Pause();

            _musicChannels[CurrentChannel].Clip = a_clip;
            Play(a_overTime);
        }

        public void ChangeMusicClip(AudioClip a_clip, float a_overTime = 1f)
        {
            if (_musicChannels[CurrentChannel].IsPlaying)
            {
                Stop(a_overTime);
            }

            SwitchChannel();

            PlayMusicClip(a_clip);
        }


        public void Play(float a_overTime = 1f)
        {
            _musicChannels[CurrentChannel].Play();
            VolumeLerpJob playJob = new VolumeLerpJob(_musicChannels[CurrentChannel], 1f, a_overTime);
            _jobSystem.AddSyncJob(playJob);
        }

        public void Pause(float a_overTime = 1f, AudioJobBase.AudioJobClbk onComplete = null)
        {
            if (!_musicChannels[CurrentChannel].IsPlaying) return;

            if (a_overTime > 0f)
            {
                AudioJobBase.AudioJobClbk clbk = _musicChannels[CurrentChannel].Pause;

                if (onComplete != null)
                {
                    clbk += onComplete;
                }

                LerpChannelVolume(_musicChannels[CurrentChannel], a_overTime, clbk);
            }
            else
            {
                _musicChannels[CurrentChannel].Pause();

                onComplete?.Invoke();
            }
        }

        public void Stop(float a_overTime = 0f, AudioJobBase.AudioJobClbk onComplete = null)
        {
            if (!_musicChannels[CurrentChannel].IsStopped) return;

            if (a_overTime > 0f)
            {
                AudioJobBase.AudioJobClbk clbk = _musicChannels[CurrentChannel].Stop;

                if (onComplete != null)
                {
                    clbk += onComplete;
                }

                LerpChannelVolume(_musicChannels[CurrentChannel], a_overTime, clbk);
            }
            else
            {
                _musicChannels[CurrentChannel].Stop();

                onComplete?.Invoke();
            }

            _musicChannels[CurrentChannel].Stop();
        }

        private void LerpChannelVolume(MusicChannel a_channel, float a_to = 1f, AudioJobBase.AudioJobClbk onComplete = null)
        {
            VolumeLerpJob lerpJob = new VolumeLerpJob(a_channel, 0f, a_to);

            if(onComplete != null)
                lerpJob.onComplete += onComplete;

            _jobSystem.AddSyncJob(lerpJob);
        }
    }
}