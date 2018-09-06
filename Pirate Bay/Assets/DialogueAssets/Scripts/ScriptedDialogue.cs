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

    public override IDialogueDisplayer Display(Speaker speaker, IIterable conversation)
    {
        return new ScriptedDialogueDisplayer(sentences, speaker, conversation);
    }
}
