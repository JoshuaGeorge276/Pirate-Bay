using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace PirateBay.World
{


    public class WorldObject : ManagedBehaviour
    {
        private int worldObjID = -1;

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

        protected override void Awake()
        {
            base.Awake();


            _gridPos.x = (int)transform.position.x;
            _gridPos.y = (int)transform.position.y;
        }

        protected override void Start()
        {
            base.Start();

            World.Instance.RegisterWorldObject(this, _gridPos);
        }

        protected override void OnEnable()
        {
            base.Start();


        }

        protected override void OnDisable()
        {
            base.OnDisable();

            World.Instance.UnregisterWorldObject(this);
        }
    }

}

