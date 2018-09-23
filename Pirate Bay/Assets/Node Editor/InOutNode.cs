using UnityEngine;
using System;

public abstract class InOutNode : DialogueNode
{

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public InOutNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
    }


    public override void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        base.Draw();
    }
    public override bool ContainsConnection(ConnectionPoint point)
    {
        return (point == inPoint) || (point == outPoint);
    }
}
