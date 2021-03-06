﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EntryNode : ConnectionNode
{
    public ConnectionPoint outPoint;

    public EntryNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, Action<ConnectionPoint> OnClickOutPoint) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        title = "Entry";
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint);
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        return point == outPoint;
    }

    public override void Draw()
    {
        base.Draw();
        outPoint.Draw();
    }

    // Does not have an inpoint. I know this is a bad implementation but must stick with it for now.
    public override ConnectionPoint GetInPoint()
    {
        throw new NotImplementedException();
    }

    public override ConnectionPoint GetOutPoint()
    {
        return outPoint;
    }
}
