using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Audio
{
    public class AudioJobSystem : ManagedBehaviour
    {
        private List<IAudioJob> _syncAudioJobs;

        private Queue<IAudioJob> _queuedAudioJobs;
        private IAudioJob _currentQueueJob;

        protected override void Awake()
        {
            base.Awake();
            _syncAudioJobs = new List<IAudioJob>();
            _queuedAudioJobs = new Queue<IAudioJob>();
        }

        public override void ManagedUpdate(float a_fDeltaTime)
        {
            RunSyncJobs(a_fDeltaTime);
            RunQueuedJobs(a_fDeltaTime);
        }

        private void RunSyncJobs(float a_fDeltaTime)
        {
            int count = _syncAudioJobs.Count;

            if (count == 0)
                return;

            IAudioJob currentJob;

            // Run Jobs
            int i = 0;
            for (; i < count; i++)
            {
                currentJob = _syncAudioJobs[i];
                currentJob.Run(a_fDeltaTime);
            }

            // Remove Done Jobs
            for (--i; i >= 0; i--)
            {
                currentJob = _syncAudioJobs[i];
                if (currentJob.IsDone())
                {
                    currentJob.OnComplete();
                    _syncAudioJobs.RemoveAt(i);
                }    
            }
        }

        public void AddSyncJob(IAudioJob a_job)
        {
            if (_syncAudioJobs.Contains(a_job))
                return;

            a_job.OnStart();
            _syncAudioJobs.Add(a_job);
        }

        private void RunQueuedJobs(float a_fDeltaTime)
        {
            if (_currentQueueJob != null)
            {
                _currentQueueJob.Run(a_fDeltaTime);

                if (_currentQueueJob.IsDone())
                {
                    _currentQueueJob.OnComplete();
                    _currentQueueJob = null;
                }
            }

            if (_currentQueueJob == null && _queuedAudioJobs.Count > 0)
            {
                _currentQueueJob = _queuedAudioJobs.Dequeue();
                _currentQueueJob.OnStart();
            }
        }

        public void QueueJob(IAudioJob a_job)
        {
            if(!_queuedAudioJobs.Contains(a_job))
                _queuedAudioJobs.Enqueue(a_job);
        }
    }
}



