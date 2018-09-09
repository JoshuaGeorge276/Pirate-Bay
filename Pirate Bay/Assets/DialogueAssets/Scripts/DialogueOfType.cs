using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOfType : IDialogueContext
{
    public DialogueTypes.Type type;

    private IDialogueContext currentContext;

    public void Init(DialogueTypes.Type type)
    {
        this.type = type;
    }

    public override void Speak(Conversation conversation, Speaker speaker)
    {
        RespondSpeaker respondSpeaker = (RespondSpeaker)speaker;
        if (respondSpeaker)
        {
            currentContext = respondSpeaker.Respond(type);
            currentContext.Speak(conversation, respondSpeaker);
        }
    }

    public override IDialogueDisplayer Display(IIterable conversation, Speaker speaker)
    {
        return currentContext.Display(conversation, speaker);
    }
}
