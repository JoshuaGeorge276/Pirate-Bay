using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConnectionNode : Node {
    public ConnectionNode(GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode) : base(nodeStyle, selectedStyle, OnClickRemoveNode)
    {
    }

    public ConnectionNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
    }

    public abstract bool ContainsConnection(ConnectionPoint point);

    public abstract ConnectionPoint GetInPoint();

    public abstract ConnectionPoint GetOutPoint();
}
