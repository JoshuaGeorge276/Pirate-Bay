using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2NPCConversation : IIterable
{

    private Speaker playerSpeaker;
    private NPCSpeaker npcSpeaker;

    private Queue<IDialogueContext> activeContexts;
    private CustomConversationLayout layout;
    private int layoutSize, currentContextIndex;

    public Player2NPCConversation(CustomConversationLayout layout, Speaker playerSpeaker, NPCSpeaker npcSpeaker)
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
        return (currentContextIndex % 2 == 0 ? playerSpeaker : npcSpeaker);
    }

    public void Next()
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
            current.Display(GetNextSpeaker(), this);
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
}
