using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitNode : Node {

    private ConnectionPoint inPoint;

    public ExitNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, GUIStyle inPointStyle, Action<ConnectionPoint> OnClickInPoint) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        title = "Exit";
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        return point == inPoint;
    }

    public override void Draw()
    {
        base.Draw();
        inPoint.Draw();
    }
}
