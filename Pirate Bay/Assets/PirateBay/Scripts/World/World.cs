using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class World : ManagedBehaviour
{
    [SerializeField]
    private int WorldSizeX, WorldSizeY;
    private WorldGrid worldGrid;

    protected override void Awake()
    {
        base.Awake();

        worldGrid = new WorldGrid(WorldSizeX, WorldSizeY);
    }

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        //throw new System.NotImplementedException();
    }

    public void RegisterWorldObjectPos(WorldObject a_obj, int x, int y)
    {

    }
}

public class WorldGrid
{
    public WorldGrid(int a_xSize, int a_ySize)
    {
        objects = new WorldObject[a_xSize, a_ySize];
    }

    public WorldObject[,] objects;
}

public interface WorldObject
{

}
