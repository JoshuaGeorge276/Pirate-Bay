using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class OptionNode : ConnectionNode
{
    // Dialogue Data
    public string text;
    public bool proceedToNextSpeaker;

    // Draw Data
    private const float padding = 15.0f;
    private Action<Node> onClickRemove;

    // Connection Points
    private ConnectionPoint outPoint;

    private Node parentNode;

    public OptionNode(int number, Node parent, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, Action<ConnectionPoint> OnClickConnectionPoint) : base(nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        title = "Option " + number;
        parentNode = parent;
        outPoint = new ConnectionPoint(parent, new Vector2(0, (number * 100.0f) - 10.0f), ConnectionPointType.Out, OnClickConnectionPoint);
        onClickRemove = OnClickRemoveNode;
    }

    public override void Draw()
    {
        GUILayout.BeginVertical(defaultNodeStyle);
        GUILayout.Space(10);
        GUILayout.Label(title);
        text = EditorGUILayout.TextField(text);
        GUILayout.BeginHorizontal();
        proceedToNextSpeaker = EditorGUILayout.Toggle("Proceed To Next Speaker ", proceedToNextSpeaker);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Remove Option"))
        {
            OnClickRemove(this);
        }
        GUILayout.Space(10);
        GUILayout.EndVertical();
        EditorGUILayout.Separator();
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        return point == outPoint;
    }

    public void OnClickRemove(Node node)
    {
        if(onClickRemove != null)
        {
            onClickRemove(this);
        }
    }

    public void DrawConnections()
    {
        outPoint.AutoDraw();
    }

    public void UpdateOrder(int order)
    {
        title = "Option " + order;
        outPoint.offset = new Vector2(0, (order * 100.0f) - 10.0f);
    }

    // Doesn't have any in points. I know this is a bad implementation but must stick with it for now.
    public override ConnectionPoint GetInPoint()
    {
        throw new NotImplementedException();
    }

    public override ConnectionPoint GetOutPoint()
    {
        return outPoint;
    }
}
