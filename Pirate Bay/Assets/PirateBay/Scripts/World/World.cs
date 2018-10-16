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

        private void Awake()
        {
            worldGrid = new WorldGrid(WorldSizeX, WorldSizeY);
            _handleTracker = new Dictionary<int, WorldGridPos>();
        }

        private void Update()
        {
            //throw new System.NotImplementedException();
        }


        public void RegisterWorldObject(WorldObject a_obj, WorldGridPos a_pos)
        {
            int id = a_obj.GetInstanceID();

            if (!_handleTracker.ContainsKey(id))
            {
                _handleTracker.Add(id, a_pos);

                worldGrid.AddObjectAtPos(a_obj, a_pos);
            }

        }

        public void UpdateWorldObjectPos(WorldObject a_obj, WorldGridPos a_pos)
        {
            int id = a_obj.GetInstanceID();

            if (_handleTracker.ContainsKey(id))
            {
                WorldGridPos currentPos = _handleTracker[id];

                worldGrid.ChangeObjectPos(currentPos, a_pos);
            }
        }

        public void UnregisterWorldObject(WorldObject a_obj)
        {
            int id = a_obj.GetInstanceID();

            if (_handleTracker.ContainsKey(id))
            {
                WorldGridPos gridPos = _handleTracker[id];

                worldGrid.RemoveObjectAtPos(gridPos);

                _handleTracker.Remove(id);
            }

        }

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
        }
    }
}


