using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsDialogueNode : DialogueData
{
    private const string title = "Options Dialogue";

    public List<OptionNode> optionNodes;

    public ConnectionPoint inPoint;

    private GUIStyle optionNodeStyle;
    [SerializeField]
    private GUIStyle optionNodeSelectedStyle;
    [SerializeField]
    private GUIStyle outPointStyle;
    [SerializeField]
    private Action<ConnectionPoint> onClickOutPoint;
    [SerializeField]
    private Action<ConnectionNode> onRemoveOptionNode;

    private Action<ConnectionPoint> onClickConnectionPoint;

    private Node parentNode;

    public OptionsDialogueNode()
    {
        optionNodeStyle = new GUIStyle();
        optionNodeStyle.normal.background = UnityEditor.EditorGUIUtility.Load("builtin skins/darkskin/images/node2.png") as Texture2D;
        optionNodeStyle.border = new RectOffset(12, 12, 12, 12);

        optionNodes = new List<OptionNode>(4);
    }

    public void SetupConnectionPoints(Node node, Action<ConnectionPoint> OnClickConnectionPoint, Action<ConnectionNode> OnRemoveConnections)
    {
        if(onClickConnectionPoint == null)
        {
            onClickConnectionPoint = OnClickConnectionPoint;
        }

        if(parentNode == null)
        {
            parentNode = node;
        }

        if(onRemoveOptionNode == null)
        {
            onRemoveOptionNode = OnRemoveConnections;
        }

        inPoint = new ConnectionPoint(node, ConnectionPointType.In, OnClickConnectionPoint);
    }

    public void Draw()
    {
        if (GUILayout.Button("Add Option"))
        {
            AddOption();
        }

        GUILayout.BeginVertical();

        foreach(OptionNode node in optionNodes)
        {
            node.Draw();
        }

        GUILayout.EndVertical();
    }

    public void AddOption()
    {
        int count = optionNodes.Count;

        if(count < 4)
        {
            OptionNode node = new OptionNode(count + 1, parentNode, optionNodeStyle, optionNodeStyle, RemoveOption, onClickConnectionPoint);
            optionNodes.Add(node);
        }
    }

    private void RemoveOption(Node option)
    {
        int index = optionNodes.FindIndex((i) => i == option);
        List<OptionNode> temp = new List<OptionNode>(4);
        onRemoveOptionNode(optionNodes[index]);
        for(int i = 0; i < optionNodes.Count; i++)
        {
            if (i == index) continue;
            int tempCount = temp.Count;
            optionNodes[i].UpdateOrder(tempCount + 1);
            temp.Add(optionNodes[i]);
        }

        optionNodes = temp;
    }

    public bool ContainsConnection(ConnectionPoint point)
    {
        bool optionContainsConnection = false;
        for(int i = 0; i < optionNodes.Count; i++)
        {
            optionContainsConnection |= optionNodes[i].ContainsConnection(point);
        }

        return (point == inPoint) || optionContainsConnection;
    }

    public IDialogueContext GetDialogueContext()
    {
        int n = optionNodes.Count;
        DialogueOption[] options = new DialogueOption[n];
        for(int i = 0; i < n; i++)
        {
            OptionNode current = optionNodes[i];
            options[i] = new DialogueOption(current.text, current.proceedToNextSpeaker);
        }

        OptionsDialogue dialogue = ScriptableObject.CreateInstance<OptionsDialogue>();
        dialogue.Init(options);

        return dialogue;
    }

    public void DrawConnections()
    {
        inPoint.Draw();

        foreach(OptionNode node in optionNodes)
        {
            node.DrawConnections();
        }
    }

    public string GetTitle()
    {
        return title;
    }

    public ConnectionPoint GetInPoint()
    {
        return inPoint;
    }

    // Doesn't contain any out points. I know this isnt a good implementation but must stick with it for now.
    public ConnectionPoint GetOutPoint()
    {
        throw new NotImplementedException();
    }

    public int GetChildCount()
    {
        return optionNodes.Count;
    }


    //public override bool HasInternalChildren()
    //{
    //    return true;
    //}

    //public override Node[] GetInternalChildren()
    //{
    //    return optionNodes.ToArray();
    //}

    //public override int GetInternalChildCount()
    //{
    //    return optionNodes.Count;
    //}
}
