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
            onStart?.Invoke();
        }

        public abstract void Run(float a_deltaTime);

        public virtual bool IsDone()
        {
            return _isDone;
        }

        public virtual void OnComplete()
        {
            onComplete?.Invoke();
        }
    }

#endregion

    public class VolumeLerpJob : AudioJobBase
    {
        private AudioChannel _channel;

        private float _lerpTime;
        private float _startVolume;
        private float _targetVolume;
        private float _timeToTarget;

        private float testTime = 0f;

        public VolumeLerpJob(AudioChannel a_channel, float a_toVolume, float a_time)
        {
            _channel = a_channel;
            _targetVolume = a_toVolume;
            _timeToTarget = a_time;
        }

        public override void OnStart()
        {
            _startVolume = _channel.Volume;

            base.OnStart();
        }

        public override void Run(float a_deltaTime)
        {
            float deltaVol = Mathf.Abs(_targetVolume - _channel.Volume);
            if (deltaVol > 0f)
            {
                _lerpTime += a_deltaTime / _timeToTarget;
                _channel.Volume = Mathf.Lerp(_startVolume, _targetVolume, _lerpTime);
                return;
            }
            _isDone = true;
        }
    }
}

