using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOfType : IDialogueContext
{
    public DialogueTypes.Type type;

    public void Init(DialogueTypes.Type type)
    {
        this.type = type;
    }

    public override IDialogueDisplayer Display(Speaker speaker, IIterable conversation)
    {
        NPCSpeaker npcSpeaker = (NPCSpeaker)speaker;
        ScriptedDialogue dialogue = npcSpeaker.ReactToDialogue(type);
        return dialogue.Display(npcSpeaker, conversation);
    }
}
