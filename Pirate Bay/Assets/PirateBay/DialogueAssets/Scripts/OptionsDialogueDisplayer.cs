using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsDialogueDisplayer : IDialogueDisplayer
{
    private Speaker speaker;
    private string[] options;
    private UnityAction<int>[] onClicks;

    public OptionsDialogueDisplayer(string[] options, UnityAction<int>[] onClicks, Speaker speaker)
    {
        this.options = options;
        this.onClicks = onClicks;
        this.speaker = speaker;

        Display();
    }

    public void Display()
    {
        // TODO: Leave this to the Context to decide.
        bool useSpecial = options.Length == 3;
        DialogueCanvasMgr.Instance.NextSpeaker(options, speaker.name, speaker.image, onClicks, useSpecial);
    }

    public void Next()
    {
        throw new System.NotImplementedException();
    }
}
