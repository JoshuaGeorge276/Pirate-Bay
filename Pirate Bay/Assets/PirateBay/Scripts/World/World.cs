using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace PirateBay.World
{
    public struct WorldGridPos
    {
        public WorldGridPos(int a_x, int a_y)
        {
            x = a_x;
            y = a_y;
        }

        public static explicit operator WorldGridPos(Vector2 a_vec)
        {
            WorldGridPos pos;
            pos.x = (int)a_vec.x;
            pos.y = (int)a_vec.y;
            return pos;
        }

        public int x;
        public int y;
    }

    public class World : SingletonBehaviour<World>
    {
        [SerializeField]
        private int WorldSizeX, WorldSizeY;
        private WorldGrid worldGrid;

        private Dictionary<int, WorldGridPos> _handleTracker;

        private WorldInteractableManager _interactableManager;

        private void Awake()
        {
            worldGrid = new WorldGrid(WorldSizeX, WorldSizeY);
            _handleTracker = new Dictionary<int, WorldGridPos>();

            _interactableManager = new WorldInteractableManager();
        }

        private bool isRegisteredWorldObj(WorldObject a_obj, out int a_id)
        {
            a_id = a_obj.GetInstanceID();

            return _handleTracker.ContainsKey(a_id);
        }

        private bool isRegisteredWorldObj(int a_id)
        {
            return _handleTracker.ContainsKey(a_id);
        }

        private bool isWorldObjAtPos(WorldGridPos a_pos, out int a_id)
        {
            return worldGrid.TryGetObjectHandleAtPos(a_pos, out a_id);
        }

        #region World Object Registration

        public void RegisterWorldObject(WorldObject a_obj, WorldGridPos a_pos)
        {
            if (isRegisteredWorldObj(a_obj, out int id))
                return;

            _handleTracker.Add(id, a_pos);
            worldGrid.AddObjectAtPos(a_obj, a_pos);
        }

        public void UnregisterWorldObject(WorldObject a_obj)
        {
            if (!isRegisteredWorldObj(a_obj, out int id))
                return;

            WorldGridPos gridPos = _handleTracker[id];
            worldGrid.RemoveObjectAtPos(gridPos);

            _interactableManager.UnregisterInteractable(id);

            _handleTracker.Remove(id);
        }

        #endregion

        public void UpdateWorldObjectPos(WorldObject a_obj, WorldGridPos a_pos)
        {
            if (!isRegisteredWorldObj(a_obj, out int id))
                return;

            WorldGridPos currentPos = _handleTracker[id];
            worldGrid.ChangeObjectPos(currentPos, a_pos);
        }

        #region Interactable

        public void RegisterInteractable(WorldObject a_obj, Interactable a_interactable)
        {
            if (!isRegisteredWorldObj(a_obj, out int id))
                return;

            _interactableManager.RegisterInteractable(id, a_interactable);
        }

        public void RegisterInteractable(int a_handle, Interactable a_interactable)
        {
            if (!isRegisteredWorldObj(a_handle))
                return;

            _interactableManager.RegisterInteractable(a_handle, a_interactable);
        }


        public void UnregisterInteractable(WorldObject a_obj)
        {
            if (!isRegisteredWorldObj(a_obj, out int id))
                return;

            _interactableManager.UnregisterInteractable(id);
        }

        public void UnregisterInteractable(int a_handle)
        {
            if (!isRegisteredWorldObj(a_handle))
                return;

            _interactableManager.UnregisterInteractable(a_handle);
        }

        public bool TryGetInteractable(WorldObject a_obj, out Interactable o_interactable)
        {
            o_interactable = null;

            if (!isRegisteredWorldObj(a_obj, out int id))
                return false;

            return _interactableManager.TryGetInteractable(id, out o_interactable);
        }

        public bool TryGetInteractable(WorldGridPos a_pos, out Interactable o_interactable)
        {
            o_interactable = null;

            if (!isWorldObjAtPos(a_pos, out int id))
                return false;

            _interactableManager.TryGetInteractable(id, out o_interactable);
            return true;
        }

        #endregion

        private class WorldGrid
        {
            private WorldObject[,] objects;

            public WorldGrid(int a_xSize, int a_ySize)
            {
                objects = new WorldObject[a_xSize, a_ySize];
            }

            public void AddObjectAtPos(WorldObject a_obj, WorldGridPos a_pos)
            {
                objects[a_pos.x, a_pos.y] = a_obj;
            }

            public void ChangeObjectPos(WorldGridPos a_currentPos, WorldGridPos a_newPos)
            {
                objects[a_newPos.x, a_newPos.y] = objects[a_currentPos.x, a_currentPos.y];
                RemoveObjectAtPos(a_currentPos);
            }

            public void RemoveObjectAtPos(WorldGridPos a_pos)
            {
                objects[a_pos.x, a_pos.y] = null;
            }

            public bool TryGetObjectAtPos(WorldGridPos a_pos, out WorldObject o_object)
            {
                o_object = objects[a_pos.x, a_pos.y];
                return o_object != null;
            }

            public bool TryGetObjectHandleAtPos(WorldGridPos a_pos, out int a_handle)
            {
                a_handle = -1;

                WorldObject worldObject = objects[a_pos.x, a_pos.y];

                if (worldObject == null)
                    return false;

                a_handle = worldObject.GetInstanceID();

                return true;
            }
        }
    }
}


