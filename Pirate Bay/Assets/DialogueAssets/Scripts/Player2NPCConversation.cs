using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2NPCConversation : Conversation
{

    private RespondSpeaker playerSpeaker;
    private RespondSpeaker npcSpeaker;

    private Queue<IDialogueContext> activeContexts;
    private CustomConversationLayout layout;
    private int layoutSize, currentContextIndex;
    private int currentSpeakerIndex;

    public Player2NPCConversation(CustomConversationLayout layout, RespondSpeaker playerSpeaker, RespondSpeaker npcSpeaker)
    {
        activeContexts = new Queue<IDialogueContext>();
        this.layout = layout;
        layoutSize = layout.Length;
        currentContextIndex = 0;
        this.playerSpeaker = playerSpeaker;
        this.npcSpeaker = npcSpeaker;

        Next();
    }

    private void GetNextContextFromLayout()
    {
        activeContexts.Enqueue(layout.contexts[currentContextIndex]);
    }

    private Speaker GetNextSpeaker()
    {
        return (currentSpeakerIndex % 2 == 0 ? playerSpeaker : npcSpeaker);
    }

    public override void Next()
    {
        if(activeContexts.Count <= 0)
        {
            if(currentContextIndex >= layoutSize)
            {
                Debug.Log("Conversation is over.");
                ConversationMgr.Instance.EndConversation();
            }
            else
            {
                GetNextContextFromLayout();
            }
        }

        if(activeContexts.Count > 0)
        {
            IDialogueContext current = activeContexts.Dequeue();
            current.Speak(this, GetNextSpeaker());
            currentContextIndex++;
        }
        else
        {
            Debug.Log("Conversation had no more contexts.");
            ConversationMgr.Instance.EndConversation();
        }
    }

    public void EnqueueContext(IDialogueContext context)
    {
        activeContexts.Enqueue(context);
    }

    public override void ProceedToNextSpeaker()
    {
        currentSpeakerIndex++;
    }
}
