using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ScriptedDialogue : IDialogueContext
{
    public string[] sentences;

    public void Init(string[] sentences)
    {
        this.sentences = sentences;
    }

    public override void Speak(Conversation conversation, Speaker speaker)
    {
        if (sentences.Length <= 0) sentences = new string[] { "..." };
        Display(conversation, speaker);
        conversation.LoadNextContext(0);
        conversation.ProceedToNextSpeaker();
    }

    public override IDialogueDisplayer Display(IIterable conversation, Speaker speaker)
    {
        return new ScriptedDialogueDisplayer(sentences, speaker, conversation);
    }

    public override bool OnlyPlayer()
    {
        return false;
    }

    public override bool ProceedToNextSpeaker()
    {
        return true;
    }
}
