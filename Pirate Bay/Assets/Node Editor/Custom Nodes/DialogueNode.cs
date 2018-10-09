using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : ConnectionNode
{
    public DialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
    }

    public abstract DialogueData GetData();

    public override abstract bool ContainsConnection(ConnectionPoint point);
}

public class DialogueNode<T> : DialogueNode where T : DialogueData, new()
{
    public Vector2 Position { get { return rect.position; } }

    public string Title { get { return Data.GetTitle(); } }

    public T Data { get; private set; }

    public DialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, Action<ConnectionPoint> OnClickConnectionPoint, Action<ConnectionNode> OnRemoveConnections) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        Data = new T();
        Data.SetupConnectionPoints(this, OnClickConnectionPoint, OnRemoveConnections);
        title = Data.GetTitle();
    }

    public override void Draw()
    {
        base.Draw();
        Data.DrawConnections();
        Rect tmp = new Rect(rect.x + 20.0f, rect.y + 20.0f, rect.width - 40.0f, rect.height - 30.0f);
        GUILayout.BeginArea(tmp);
        GUILayout.BeginScrollView(tmp.position);
        Data.Draw();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        return Data.ContainsConnection(point);
    }

    public override DialogueData GetData()
    {
        return Data;
    }

    public override ConnectionPoint GetInPoint()
    {
        return Data.GetInPoint();
    }

    public override ConnectionPoint GetOutPoint()
    {
        return Data.GetOutPoint();
    }
}
