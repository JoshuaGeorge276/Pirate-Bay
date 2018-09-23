using System;
using UnityEngine;

namespace Core
{
    public abstract class ManagedBehaviour : MonoBehaviour
    {
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