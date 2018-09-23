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

    private const float padding = 15.0f;
    private Action<Node> onClickRemove;

    public OptionNode(int optionNum, Vector2 position, float width, float height, GUIStyle nodeStyle, Action<Node> OnClickRemoveNode) : base(position, width, height, nodeStyle, nodeStyle, OnClickRemoveNode)
    {
        title = "Option " + optionNum;
        onClickRemove = OnClickRemoveNode;
    }

    public override void Draw()
    {
        base.Draw();
        GUILayout.BeginArea(new Rect(rect.x + padding, rect.y + padding * 2.0f, rect.width - padding * 2.0f, rect.height - padding));
        text = EditorGUILayout.TextField(text);
        GUILayout.BeginHorizontal();
        proceedToNextSpeaker = EditorGUILayout.Toggle("Proceed To Next Speaker ", proceedToNextSpeaker);
        endConversation = EditorGUILayout.Toggle("End Conversation? ", endConversation);
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Remove Option"))
        {
            onClickRemove(this);
        }
        GUILayout.EndArea();
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        return false;
    }
}
