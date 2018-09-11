using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class BehaviourManager : SingletonBehaviour<BehaviourManager>, EnvironmentModule
    {
        private const int maxBehaviours = 1000;

        private List<ManagedBehaviour> _Behaviours;
        private List<ManagedBehaviour> _regularBehaviours;
        private List<ManagedBehaviour> _fixedBehaviours;
        private List<ManagedBehaviour> _lateBehaviours;

        private float _deltaTime;
        private float _fixedTime;

        public void Load()
        {
            _Behaviours = new List<ManagedBehaviour>(maxBehaviours);
            _regularBehaviours = new List<ManagedBehaviour>();
            _fixedBehaviours = new List<ManagedBehaviour>();
            _lateBehaviours = new List<ManagedBehaviour>();
        }

        public void PostLoad()
        {
            
        }

        // Update is called once per frame
        public void Update()
        {
            _deltaTime = Time.deltaTime;

            foreach (ManagedBehaviour behaviour in _regularBehaviours)
            {
                if (behaviour.isActiveAndEnabled)
                    behaviour.ManagedUpdate(_deltaTime);
            }
        }

        public void LateUpdate()
        {
            foreach (ManagedBehaviour behaviour in _lateBehaviours)
            {
                if (behaviour.isActiveAndEnabled)
                    behaviour.ManagedLateUpdate(_deltaTime);
            }
        }

        public void FixedUpdate()
        {
            _fixedTime = Time.fixedDeltaTime;

            foreach (ManagedBehaviour behaviour in _fixedBehaviours)
            {
                if (behaviour.isActiveAndEnabled)
                    behaviour.ManagedFixedUpdate(_fixedTime);
            }
        }

        public void RegisterBehaviour(ManagedBehaviour a_behaviour)
        {
            if (_Behaviours.Find(existing => existing.Equals(a_behaviour)))
            {
                Debug.Log("Attempted to add existing behaviour to manager!");
                return;
            }

            _Behaviours.Add(a_behaviour);

            if (a_behaviour.GetType().GetMethod("ManagedUpdate").isOverridden())
            {
                _regularBehaviours.Add(a_behaviour);
            }

            if (a_behaviour.GetType().GetMethod("ManagedLateUpdate").isOverridden())
            {
                _lateBehaviours.Add(a_behaviour);
            }

            if (a_behaviour.GetType().GetMethod("ManagedFixedUpdate").isOverridden())
            {
                _fixedBehaviours.Add(a_behaviour);
            }
        }

        public void DeregisterBehaviour(ManagedBehaviour a_behaviour)
        {
            if (!_Behaviours.Find(existing => existing.Equals(a_behaviour)))
            {
                Debug.Log("Attempted to deregister non existent behaviour from manager!");
                return;
            }

            _Behaviours.Remove(a_behaviour);
        }
    }
}