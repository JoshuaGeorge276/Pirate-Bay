using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode<T> : ConnectionNode where T : DialogueData, new()
{
    public Vector2 Position { get { return rect.position; } }

    private T dialogueData;

    public DialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, Action<ConnectionPoint> OnClickConnectionPoint) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        dialogueData = new T();
        dialogueData.SetupConnectionPoints(this, OnClickConnectionPoint);
    }

    public override void Draw()
    {
        base.Draw();
        dialogueData.DrawConnections();
        Rect tmp = new Rect(rect.x + 20.0f, rect.y + 20.0f, rect.width - 40.0f, rect.height - 30.0f);
        GUILayout.BeginArea(tmp);
        GUILayout.BeginScrollView(tmp.position);
        dialogueData.Draw();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        return dialogueData.ContainsConnection(point);
    }
}
