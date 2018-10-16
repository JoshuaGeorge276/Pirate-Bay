using System;
using UnityEngine;

namespace Core
{
    public abstract class ManagedBehaviour : MonoBehaviour
    {
        protected virtual void Awake() {}

        protected virtual void Start() {}

        public virtual void ManagedUpdate(float a_fDeltaTime) {}
        public virtual void ManagedLateUpdate(float a_fDeltaTime) {}
        public virtual void ManagedFixedUpdate(float a_fDeltaTime) {}

        protected virtual void OnEnable()
        {
            BehaviourManager.Instance.RegisterBehaviour(this);
        }

        protected virtual void OnDisable()
        {
            BehaviourManager.Instance.DeregisterBehaviour(this);
        }
        protected virtual void OnDestroy() {}

        protected virtual void OnDrawGizmos() {}
    }
}