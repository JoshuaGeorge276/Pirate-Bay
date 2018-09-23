using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class TypedDialogueNode : InOutNode
{

    private const float padding = 10.0f;

    [SerializeField]
    DialogueTypes.Type type = DialogueTypes.Type.Greeting;


    public TypedDialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint)
    {
        title = "Typed Dialogue";
    }

    public override void Draw() 
    {
        base.Draw();
        Rect enumOptions = new Rect(rect.x + padding, rect.y + 20.0f, rect.width - padding * 2, 20.0f);
        type = (DialogueTypes.Type)EditorGUI.EnumPopup(enumOptions, type);

    }

    public override bool HasInternalChildren()
    {
        return false;
    }

    public override IDialogueContext GetDialogueContext()
    {
        DialogueOfType dialogue = ScriptableObject.CreateInstance<DialogueOfType>();
        dialogue.Init(type);
        return dialogue;
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
