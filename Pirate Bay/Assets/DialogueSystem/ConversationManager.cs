using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RockFace.DialogueSystem;

public class ConversationManager : MonoBehaviour {

    public TextBox textBox;
    public Text speakerName;
    public Image speakerImage;

    public BasicSpeaker speaker1;
    public BasicSpeaker speaker2;

    private Queue<string> dialogueQueue = new Queue<string>();
    private ScriptedConversation currentConversation;

    public void StartScriptedConversation(ScriptedConversationLayout layout)
    {
        ScriptedConversation conversation = new ScriptedConversation(layout, speaker1, speaker2);
        currentConversation = conversation;
        NextSpeakerDialogue();
        textBox.DisplayDialogue(dialogueQueue.Dequeue());
    }

    public void OnNext()
    {
        if (!currentConversation.Finished())
        {
            if (dialogueQueue.Count <= 0)
            {
                NextSpeakerDialogue();
            }
        }
        else if(dialogueQueue.Count <= 0)
        {
            textBox.DisplayDialogue("Conversation has ended!");
            return;
        }

        textBox.DisplayDialogue(dialogueQueue.Dequeue());
    }

    private void NextSpeakerDialogue()
    {
        if (!currentConversation.Finished())
        {
            BasicSpeaker currentSpeaker = currentConversation.GetCurrentSpeaker();
            speakerName.text = currentSpeaker.name;
            speakerImage.sprite = currentSpeaker.image;

            GenericDialogue context = (GenericDialogue)currentConversation.GetNext();
            string[] newDialogue = context.dialogue;
            for (int i = 0; i < newDialogue.Length; i++)
            {
                dialogueQueue.Enqueue(newDialogue[i]);
            }
        }
    }
}
