using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.Events;

namespace PirateBay.World
{
    public class WorldObject : ManagedBehaviour
    {
        public int GetHandle()
        {
            return GetInstanceID();
        }

        private WorldGridPos _gridPos;

        public WorldGridPos GridPos
        {
            get { return _gridPos; }
            set
            {
                _gridPos = value;
                World.Instance.UpdateWorldObjectPos(this, _gridPos);
            }
        }

        public UnityAction OnEnabled;
        public UnityAction OnDisabled;

        protected override void Awake()
        {
            _gridPos.x = (int) transform.localPosition.x;
            _gridPos.y = (int) transform.localPosition.y;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            World.Instance.RegisterWorldObject(this, _gridPos);
            OnEnabled?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnDisabled?.Invoke();
            World.Instance.UnregisterWorldObject(this);
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }

    }

}

