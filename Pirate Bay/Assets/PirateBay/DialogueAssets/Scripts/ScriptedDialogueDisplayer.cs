using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedDialogueDisplayer : IDialogueDisplayer
{
    private Speaker speaker;
    private Queue<string> dialogueQueue = new Queue<string>();
    private IIterable conversation;

    public ScriptedDialogueDisplayer(string[] dialogue, Speaker speaker, IIterable conversation)
    {
        this.speaker = speaker;
        this.conversation = conversation;

        foreach(string sentence in dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        Display();
    }


    public void Display()
    {
        DialogueCanvasMgr.Instance.NextSpeaker(dialogueQueue, speaker.name, speaker.image, conversation.Next);
    }

    public void Next()
    {
        throw new System.NotImplementedException();
    }
}
