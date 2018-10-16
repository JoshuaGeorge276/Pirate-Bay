using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{

#region Base

    public interface IAudioJob
    {
        void OnStart();
        void Run(float a_deltaTime);
        bool IsDone();
        void OnComplete();
    }

    public abstract class AudioJobBase : IAudioJob
    {
        public delegate void AudioJobClbk();

        public event AudioJobClbk onStart;
        public event AudioJobClbk onComplete;

        protected bool _isDone = false;

        public virtual void OnStart()
        {
            if (onStart != null) onStart();
        }

        public abstract void Run(float a_deltaTime);

        public virtual bool IsDone()
        {
            return _isDone;
        }

        public virtual void OnComplete()
        {
            if (onComplete != null) onComplete();
        }
    }

#endregion

    public class VolumeLerpJob : AudioJobBase
    {
        private AudioSource _source;

        private float _lerpTime;
        private float _startVolume;
        private float _targetVolume;
        private float _timeToTarget;

        private float testTime = 0f;

        public VolumeLerpJob(AudioSource a_source, float a_toVolume, float a_time)
        {
            _source = a_source;
            _targetVolume = a_toVolume;
            _timeToTarget = a_time;
        }

        public override void OnStart()
        {
            _startVolume = _source.volume;

            base.OnStart();
        }

        public override void Run(float a_deltaTime)
        {
            float deltaVol = Mathf.Abs(_targetVolume - _source.volume);
            if (deltaVol > 0f)
            {
                _lerpTime += a_deltaTime / _timeToTarget;
                _source.volume = Mathf.Lerp(_startVolume, _targetVolume, _lerpTime);
                return;
            }
            _isDone = true;
        }
    }
}

