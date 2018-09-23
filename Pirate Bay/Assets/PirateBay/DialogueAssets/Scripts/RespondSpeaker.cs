using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespondSpeaker : Speaker {

    public NPCDialogueBundle dialogueData;

    private void Start()
    {
    }

    public virtual IDialogueContext Respond(DialogueTypes.Type type)
    {
        return dialogueData.GetDialogueOfType(type);
    }

}
