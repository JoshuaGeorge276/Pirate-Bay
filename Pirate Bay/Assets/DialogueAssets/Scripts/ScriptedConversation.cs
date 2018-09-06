using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedConversation : IIterable {

    private Speaker[] speakers;
    private ScriptedConversationLayout layout;
    private int dialogueCount = 0;
    private int currentDialogue = 0;

    public ScriptedConversation(Speaker[] speakers, ScriptedConversationLayout layout)
    {
        this.speakers = new Speaker[layout.SpeakerCount];
        InitialiseSpeakers(speakers);
        dialogueCount = layout.DialogueCount;
        this.layout = layout;
        Next();
    }

    public void Next()
    {
        if(currentDialogue < dialogueCount)
        {
            ScriptedDialogue dialogue = layout.GetNextDialogue(currentDialogue);
            Speaker speaker = speakers[layout.GetNextSpeaker(currentDialogue)];
            dialogue.Display(speaker, this);
            currentDialogue++;
        }
        else
        {
            Debug.Log("Conversation is over.");
            ConversationMgr.Instance.EndConversation();
        }
    }

    private void InitialiseSpeakers(Speaker[] speakers)
    {
        if(speakers.Length < this.speakers.Length) { Debug.LogError("There are not enough speakers for this conversation!"); throw new System.Exception(); }

        for(int i = 0; i < this.speakers.Length; i++)
        {
            this.speakers[i] = speakers[i];
        }
    }



}
