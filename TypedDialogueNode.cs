using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class TypedDialogueNode : DialogueData
{
    private const string title = "Typed Dialogue";

    // Dialogue Data
    public DialogueTypes.Type type = DialogueTypes.Type.Greeting;
    public bool proceedToNextSpeaker = true;

    // Draw Data
    [SerializeField]
    private const float padding = 10.0f;

    // Connection Points
    private ConnectionPoint inPoint;
    private ConnectionPoint outPoint;

    public TypedDialogueNode()
    {
    }
    public void SetupConnectionPoints(Node node, Action<ConnectionPoint> OnClickConnectionPoint, Action<ConnectionNode> OnRemoveConnections)
    {
        inPoint = new ConnectionPoint(node, ConnectionPointType.In, OnClickConnectionPoint);
        outPoint = new ConnectionPoint(node, ConnectionPointType.Out, OnClickConnectionPoint);
    }

    public void Draw() 
    {
        type = (DialogueTypes.Type)EditorGUILayout.EnumPopup(type);
        proceedToNextSpeaker = EditorGUILayout.Toggle(proceedToNextSpeaker);
    }

    public IDialogueContext GetDialogueContext()
    {
        DialogueOfType dialogue = ScriptableObject.CreateInstance<DialogueOfType>();
        dialogue.Init(type, proceedToNextSpeaker);
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

    //public override bool HasInternalChildren()
    //{
    //    return false;
    //}

    //public override Node[] GetInternalChildren()
    //{
    //    // Scripted Dialogue Node has no internal children. This should not be called unless has internal children is true.
    //    throw new Exception("Scripted Dialogue Node has no internal children. This method should not be called unless has internal children is true");
    //}

    //public override int GetInternalChildCount()
    //{
    //    return 0;
    //}

}
