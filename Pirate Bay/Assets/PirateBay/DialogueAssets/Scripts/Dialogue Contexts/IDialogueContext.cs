using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IDialogueContext : ScriptableObject
{

    public abstract bool OnlyPlayer();
    public abstract bool ProceedToNextSpeaker();
    public abstract void Speak(Conversation conversation, Speaker speaker);
    public abstract IDialogueDisplayer Display(IIterable conversation, Speaker speaker);
}