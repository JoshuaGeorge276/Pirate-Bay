using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OptionNode : Node
{
    public string text;
    public bool proceedToNextSpeaker;
    public bool endConversation;

    private ConnectionPoint outPoint;

    private const float padding = 15.0f;
    private Action<Node> onClickRemove;

    public OptionNode(int optionNum, Vector2 position, float width, float height, GUIStyle nodeStyle, Action<Node> OnClickRemoveNode, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickOutPoint) : base(position, width, height, nodeStyle, nodeStyle, OnClickRemoveNode)
    {
        title = "Option " + optionNum;
        titleStyle.fontStyle = FontStyle.Normal;
        titleStyle.alignment = TextAnchor.UpperLeft;
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        onClickRemove = OnClickRemoveNode;
    }

    public override void Draw()
    {
        titleRect = rect;
        titleRect.y += 10;
        titleRect.x += 10;
        GUI.Box(rect, "", style);
        GUI.Label(titleRect, new GUIContent(title), titleStyle);
        outPoint.Draw();
        GUILayout.BeginArea(new Rect(rect.x + padding, rect.y + padding * 2.0f, rect.width - padding * 2.0f, rect.height - padding));
        text = EditorGUILayout.TextField(text);
        GUILayout.BeginHorizontal();
        proceedToNextSpeaker = EditorGUILayout.Toggle("Proceed To Next Speaker ", proceedToNextSpeaker);
        endConversation = EditorGUILayout.Toggle("End Conversation? ", endConversation);
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Remove Option"))
        {
            OnClickRemove(this);
        }
        GUILayout.EndArea();
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
}
