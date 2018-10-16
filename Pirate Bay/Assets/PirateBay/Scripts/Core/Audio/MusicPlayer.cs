using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{
    public class MusicPlayer
    {
        private AudioJobSystem _jobSystem;
        private AudioSource _musicSource;


        private AudioClip _currentMusicClip;

        public MusicPlayer(AudioSource a_musicSource, AudioJobSystem a_jobSystem)
        {
            _musicSource = a_musicSource;
            _musicSource.spatialBlend = 0f;

            _jobSystem = a_jobSystem;
        }

        public bool isPlaying()
        {
            return _musicSource.isPlaying;
        }

        public bool isPlayingClip(AudioClip a_clip)
        {
            if (!_musicSource.isPlaying)
                return false;

            return _musicSource.clip == a_clip;
        }

        public void PlayMusicClip(AudioClip a_clip)
        {
            _currentMusicClip = a_clip;
            _musicSource.Pause();
            _musicSource.clip = _currentMusicClip;
            _musicSource.Play();
        }


        public void Play()
        {
            _musicSource.Play();
            VolumeLerpJob playJob = new VolumeLerpJob(_musicSource, 1f, 1f);
            _jobSystem.AddSyncJob(playJob);
        }

        public void Pause(AudioJobBase.AudioJobClbk onComplete = null)
        {
            VolumeLerpJob pauseJob = new VolumeLerpJob(_musicSource, 0f, 1f);
            pauseJob.onComplete += _musicSource.Pause;
            if(onComplete != null)
                pauseJob.onComplete += onComplete;

            _jobSystem.AddSyncJob(pauseJob);
        }

        public void Stop(bool immiediately = false)
        {
            _musicSource.Stop();
        }

        public void Reset()
        {
            Pause(InternalReset);
            _currentMusicClip = null;
        }

        private void InternalReset()
        {
            _musicSource.Stop();
            _currentMusicClip = null;
        }

    }
}