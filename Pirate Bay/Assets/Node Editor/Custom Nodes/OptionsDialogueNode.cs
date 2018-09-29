using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsDialogueNode : DialogueData
{
    public ConnectionPoint inPoint;

    private GUIStyle optionNodeStyle;
    [SerializeField]
    private GUIStyle optionNodeSelectedStyle;
    [SerializeField]
    private GUIStyle outPointStyle;
    [SerializeField]
    private Action<ConnectionPoint> onClickOutPoint;
    [SerializeField]
    public List<OptionNode> optionNodes;
    [SerializeField]
    private Action<Node> onRemoveOptionNode;

    private Action<ConnectionPoint> onClickConnectionPoint;

    public OptionsDialogueNode()
    {
        optionNodeStyle = new GUIStyle();
        optionNodeStyle.normal.background = UnityEditor.EditorGUIUtility.Load("builtin skins/darkskin/images/node2.png") as Texture2D;
        optionNodeStyle.border = new RectOffset(12, 12, 12, 12);

        optionNodes = new List<OptionNode>(4);
    }

    public void SetupConnectionPoints(Node node, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        if(onClickConnectionPoint == null)
        {
            onClickConnectionPoint = OnClickConnectionPoint;
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

    private void AddOption()
    {
        int count = optionNodes.Count;

        if(count < 4)
        {
            OptionNode node = new OptionNode(count + 1, optionNodeStyle, optionNodeStyle, RemoveOption, onClickConnectionPoint);
            optionNodes.Add(node);
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
            optionNodes[i].title = "Option " + (tempCount + 1);
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
