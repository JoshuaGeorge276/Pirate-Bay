using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Speaker Dialogue Data")]
public class SpeakerDialogueData : ScriptableObject {

    [System.Serializable]
    public struct Data
    {
        public DialogueTypes.Type type;
        public IDialogueContext[] context;
    }
        
    public Data[] data;
    private Dictionary<DialogueTypes.Type, IDialogueContext[]> dialogueDic;

    public void Init()
    {
        dialogueDic = new Dictionary<DialogueTypes.Type, IDialogueContext[]>();

        foreach (Data d in data)
        {
            dialogueDic.Add(d.type, d.context);
        }
    }

    public IDialogueContext GetContextOfType(DialogueTypes.Type type, int index = 0)
    {
        index = Clamp(type, index);
        if (dialogueDic.ContainsKey(type))
            return dialogueDic[type][index];
        else
            return CreateInstance<ScriptedDialogue>();
    }

    private int Clamp(DialogueTypes.Type type, int index = 0)
    {
        int contextLength = dialogueDic[type].Length;
        return (index > contextLength) ? contextLength : index;
    }
}
