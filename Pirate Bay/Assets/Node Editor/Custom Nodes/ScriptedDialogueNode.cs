using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ScriptedDialogueNode : DialogueData
{
    private const string title = "Scripted Dialogue";

    // Dialogue Data
    public List<string> sentences = new List<string>();

    // Draw Data
    private const float padding = 10.0f;
    private float defaultHeight;

    // Connection Points
    private ConnectionPoint inPoint;
    private ConnectionPoint outPoint;

    public ScriptedDialogueNode()
    {
        // Adding an initial sentence because all scripted dialogue should have at least one sentence.
        sentences.Add("");
    }

    public void SetupConnectionPoints(Node node, Action<ConnectionPoint> OnClickConnectionPoint, Action<ConnectionNode> OnRemoveConnections)
    {
        inPoint = new ConnectionPoint(node, ConnectionPointType.In, OnClickConnectionPoint);
        outPoint = new ConnectionPoint(node, ConnectionPointType.Out, OnClickConnectionPoint);
    }

    public void Draw()
    {
        if (GUILayout.Button("Add Sentence", GUILayout.ExpandWidth(false)))
        {
            sentences.Add("");
        }

        for(int i = 0; i < sentences.Count; i++)
        {
            sentences[i] = GUILayout.TextArea(sentences[i]);
        }
    }

    public IDialogueContext GetDialogueContext()
    {
        ScriptedDialogue dialogue = ScriptableObject.CreateInstance<ScriptedDialogue>();
        dialogue.Init(sentences.ToArray());
        return dialogue;
    }

    public bool ContainsConnection(ConnectionPoint point)
    {
        return (inPoint == point) || (outPoint == point);
    }

    public void DrawConnections()
    {
        inPoint.Draw();
        outPoint.Draw();
    }

    public string GetTitle()
    {
        return title;
    }

    public ConnectionPoint GetInPoint()
    {
        return inPoint;
    }

    public ConnectionPoint GetOutPoint()
    {
        return outPoint;
    }

    public int GetChildCount()
    {
        return 1;
    }

}
