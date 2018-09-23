using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ScriptedDialogueNode : InOutNode
{
    [SerializeField]
    private List<string> sentences = new List<string>();

    private const float padding = 10.0f;
    private float defaultHeight;

    public ScriptedDialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint)
    {
        title = "Scripted Dialogue";
        defaultHeight = height;
    }

    public override void Draw()
    {
        base.Draw();

        GUILayout.BeginArea(new Rect(rect.x + padding, rect.y + padding * 2.0f, rect.width - padding * 2.0f, rect.height - padding));

        if (GUILayout.Button("Add Sentence"))
        {
            sentences.Add("");
            rect.height += 20.0f;
        }

        for(int i = 0; i < sentences.Count; i++)
        {
            sentences[i] = GUILayout.TextArea(sentences[i]);
        }

        GUILayout.EndArea();
    }

    public override IDialogueContext GetDialogueContext()
    {
        ScriptedDialogue dialogue = ScriptableObject.CreateInstance<ScriptedDialogue>();
        dialogue.Init(sentences.ToArray());
        return dialogue;
    }

    public override bool HasInternalChildren()
    {
        return false;
    }

    public override Node[] GetInternalChildren()
    {
        // Scripted Dialogue Node has no internal children. This should not be called unless has internal children is true.
        throw new Exception("Scripted Dialogue Node has no internal children. This method should not be called unless has internal children is true");
    }

    public override int GetInternalChildCount()
    {
        return 0;
    }
}
