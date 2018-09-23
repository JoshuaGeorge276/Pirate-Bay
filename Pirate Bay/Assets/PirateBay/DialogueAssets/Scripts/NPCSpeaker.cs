using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeaker : Speaker {

    public NPCDialogueBundle dialogueBundle;

    public ScriptedDialogue ReactToDialogue(DialogueTypes.Type type)
    {
        return dialogueBundle.GetDialogueOfType(type);
    }

}
