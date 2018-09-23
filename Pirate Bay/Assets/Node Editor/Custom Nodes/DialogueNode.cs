using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : Node {
    public DialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, Action<Node> OnClickRemoveNode) : base(position, width, height, nodeStyle, selectedStyle, OnClickRemoveNode)
    {
    }

    public abstract IDialogueContext GetDialogueContext();

    public abstract bool HasInternalChildren();

    public abstract Node[] GetInternalChildren();

    public abstract int GetInternalChildCount();
}
