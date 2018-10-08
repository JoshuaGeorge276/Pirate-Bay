using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOfType : IDialogueContext
{
    [SerializeField]
    public DialogueTypes.Type type;

    public bool proceedToNextSpeaker = true;

    private IDialogueContext currentContext;

    public void Init(DialogueTypes.Type type, bool proceedToNextSpeaker)
    {
        this.type = type;
        this.proceedToNextSpeaker = proceedToNextSpeaker;
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

    public override bool OnlyPlayer()
    {
        return false;
    }

    public override bool ProceedToNextSpeaker()
    {
        return proceedToNextSpeaker;
    }
}