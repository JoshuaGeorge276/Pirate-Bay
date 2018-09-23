using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConversationLayout : ScriptableObject {

    public string title;

    public List<IDialogueContext> contexts;
    public List<int> childCounts;

    public void Init(string title, List<IDialogueContext> contexts, List<int> childCounts)
    {
        this.title = title;
        this.contexts = contexts;
        this.childCounts = childCounts;

        int count = 0;
        foreach(IDialogueContext context in contexts)
        {
            UnityEditor.AssetDatabase.CreateAsset(context, "Assets/DialogueObjects/" + title + "Context" + ++count + ".asset");
        }

        UnityEditor.AssetDatabase.SaveAssets();
    }

    public TreeList<IDialogueContext> GetDialogueTree()
    {
        TreeList<IDialogueContext> dialogueTree = new TreeList<IDialogueContext>();

        for (int i = 0; i < contexts.Count; i++)
        {
            dialogueTree.Insert(new TreeNode<IDialogueContext>(contexts[i], childCounts[i]));
        }

        return dialogueTree;
    }

}
