using System;
using UnityEngine;

namespace Core
{
    [Flags]
    public enum ManagedBehaviourFlag
    {
        None = 0x0,
        Regular = 0x2,
        Fixed = 0x4,
        Late = 0x6
    }

    public abstract class ManagedBehaviour : MonoBehaviour
    {
        [HideInInspector] public ManagedBehaviourFlag Flag = ManagedBehaviourFlag.None;

        protected virtual void Awake() {}
        protected virtual void Start()
        {
            BehaviourManager.Instance.RegisterBehaviour(this);
        }

        public abstract void ManagedUpdate(float a_fDeltaTime);
        public virtual void ManagedLateUpdate(float a_fDeltaTime) {}
        public virtual void ManagedFixedUpdate(float a_fDeltaTime) {}

        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}
        protected virtual void OnDestroy() {}
    }
}