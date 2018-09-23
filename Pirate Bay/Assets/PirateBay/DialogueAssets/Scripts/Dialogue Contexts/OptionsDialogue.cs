using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[System.Serializable]
public class OptionsDialogue : IDialogueContext {

    [SerializeField]
    public DialogueOption[] options = new DialogueOption[4];

    public void Init(DialogueOption[] options)
    {
        this.options = options;
    }

    private Conversation conversation;

    public override void Speak(Conversation conversation, Speaker speaker)
    {
        this.conversation = conversation;
        Display(conversation, speaker);
    }

    public void OptionSelected(int i)
    {
        conversation.LoadNextContext(i);
        if (options[i].proceedToNextSpeaker)
        {
            conversation.ProceedToNextSpeaker();
        }

        conversation.Next();
    }

    public override IDialogueDisplayer Display(IIterable conversation, Speaker speaker)
    {
        int optionsCount = ValidOptions();
        string[] optionsText = new string[optionsCount];
        UnityAction<int>[] onClicks = new UnityAction<int>[optionsCount];

        for (int i = 0; i < optionsCount; i++)
        {
            optionsText[i] = options[i].dialogue;
            onClicks[i] = OptionSelected;
        }

        return new OptionsDialogueDisplayer(optionsText, onClicks, speaker);
    }

    private int ValidOptions()
    {
        int count = 0;
        for(int i = 0; i < options.Length; i++)
        {
            if (options[i].IsValid()) count++;
        }

        return count;
    }
}

[System.Serializable]
public class DialogueOption
{
    public string dialogue;
    public bool endConversation;
    public bool proceedToNextSpeaker;

    public DialogueOption(string dialogue, bool endConvo, bool proceedToNextSpeaker)
    {
        this.dialogue = dialogue;
        endConversation = endConvo;
        this.proceedToNextSpeaker = proceedToNextSpeaker;
    }

    public bool IsValid() { return dialogue.Length > 0; }

}
