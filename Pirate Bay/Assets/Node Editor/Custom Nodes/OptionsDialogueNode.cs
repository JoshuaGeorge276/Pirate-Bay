﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsDialogueNode : DialogueNode
{

    public ConnectionPoint inPoint; 

    private const float padding = 10.0f;
    private float optionWidth, optionHeight;
    private Vector2 optionStartPos;
    private GUIStyle optionNodeStyle;
    private GUIStyle optionNodeSelectedStyle;
    private GUIStyle outPointStyle;
    private Action<ConnectionPoint> onClickOutPoint;
    private List<OptionNode> optionNodes;
    private List<ConnectionPoint> optionOutPoints;


    public OptionsDialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, GUIStyle inPointStyle, Action<ConnectionPoint> OnClickInPoint, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnRemoveOptionNode) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
        title = "Options Node";
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        optionNodeStyle = new GUIStyle();
        optionNodeStyle.normal.background = UnityEditor.EditorGUIUtility.Load("builtin skins/darkskin/images/node2.png") as Texture2D;
        optionNodeStyle.border = new RectOffset(12, 12, 12, 12);

        this.outPointStyle = outPointStyle;
        onClickOutPoint = OnClickOutPoint;

        optionNodes = new List<OptionNode>(4);
        optionOutPoints = new List<ConnectionPoint>(4);
    }

    public override void Drag(Vector2 delta)
    {
        base.Drag(delta);

        foreach (OptionNode node in optionNodes)
        {
            node.Drag(delta);
        }
    }

    public override void Draw()
    {
        inPoint.Draw();
        GUI.Box(rect, title, style);

        if(GUI.Button(new Rect(rect.x + padding, rect.y + padding * 2.0f, 100, 25), "Add Option"))
        {
            AddOption();
        }

        foreach(OptionNode node in optionNodes)
        {
            node.Draw();
        }

        for(int i = 0; i < optionOutPoints.Count; i++)
        {
            optionOutPoints[i].Draw();
        }

        rect.height = (optionNodes.Count * 100.0f) + 75.0f;
    }

    private void AddOption()
    {
        int count = optionNodes.Count;

        if(count < 4)
        {
            optionStartPos = new Vector2(rect.x + padding, rect.y + padding * 5);
            Vector2 pos = new Vector2(optionStartPos.x, optionStartPos.y + (count * 101));
            OptionNode node = new OptionNode(count + 1, pos, rect.width - padding * 2.0f, 100.0f, optionNodeStyle, RemoveOption);
            optionNodes.Add(node);
            optionOutPoints.Add(new ConnectionPoint(node, ConnectionPointType.Out, outPointStyle, onClickOutPoint));
        }
    }

    private void RemoveOption(Node option)
    {
        int index = optionNodes.FindIndex((i) => i == option);
        List<OptionNode> temp = new List<OptionNode>(4);
        for(int i = 0; i < optionNodes.Count; i++)
        {
            if (i == index) continue;
            int tempCount = temp.Count;
            optionStartPos = new Vector2(rect.x + padding, rect.y + padding * 5);
            Vector2 pos = new Vector2(optionStartPos.x, optionStartPos.y + (tempCount * 101));
            optionNodes[i].rect.position = pos;
            optionNodes[i].title = "Option " + (tempCount + 1);
            temp.Add(optionNodes[i]);
        }

        for(int i = index; i < optionOutPoints.Count - 1; i++)
        {
            optionOutPoints[i] = optionOutPoints[i - 1];
        }

        // Removes the connection associated with this node.
        if (OnRemoveNode != null) OnRemoveNode(option);

        optionNodes = temp;
    }

    public override bool ContainsConnection(ConnectionPoint point)
    {
        // TODO: Check if the point is equal to any option outPoints.
        return point == inPoint;
    }

    public override IDialogueContext GetDialogueContext()
    {
        int n = optionNodes.Count;
        DialogueOption[] options = new DialogueOption[n];
        for(int i = 0; i < n; i++)
        {
            OptionNode current = optionNodes[i];
            options[i] = new DialogueOption(current.text, current.endConversation, current.proceedToNextSpeaker);
        }

        OptionsDialogue dialogue = ScriptableObject.CreateInstance<OptionsDialogue>();
        dialogue.Init(options);

        return dialogue;
    }

    public override bool HasInternalChildren()
    {
        return true;
    }

    public override Node[] GetInternalChildren()
    {
        return optionNodes.ToArray();
    }

    public override int GetInternalChildCount()
    {
        return optionNodes.Count;
    }
}
