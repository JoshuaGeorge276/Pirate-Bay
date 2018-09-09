using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespondSpeaker : Speaker {

    public SpeakerDialogueData dialogueData;

    private void Start()
    {
        dialogueData.Init();
    }

    public virtual IDialogueContext Respond(DialogueTypes.Type type)
    {
        return dialogueData.GetContextOfType(type);
    }

}
