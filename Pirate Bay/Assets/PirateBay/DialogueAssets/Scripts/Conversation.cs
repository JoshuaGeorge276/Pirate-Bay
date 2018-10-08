using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : IIterable
{

    private int playerSpeaker;

    private TreeList<IDialogueContext> dialogueTree;
    private TreeNode<IDialogueContext> currentNode;

    private Queue<IDialogueContext> contexts;

    private Speaker[] speakers;

    private int speakerCount;
    private int currentSpeakerIndex;

    public Conversation(TreeList<IDialogueContext> dialogueTree, Speaker[] speakers, int playerSpeakerOrder = 0)
    {
        this.dialogueTree = dialogueTree;
        playerSpeaker = playerSpeakerOrder;
        this.speakers = speakers;
        speakerCount = speakers.Length;
        contexts = new Queue<IDialogueContext>();
    }

    public void StartConversation()
    {
        currentSpeakerIndex = 0;
        currentNode = dialogueTree.root;
        contexts.Enqueue(currentNode.node);
        Next();
    }

    public void Next()
    {
        if (contexts.Count > 0)
        {
            IDialogueContext current = contexts.Dequeue();
            current.Speak(this, GetSpeaker(current));
        }
        else
        {
            Debug.Log("Conversation has no more contexts");
            ConversationMgr.Instance.EndConversation();
        }
    }

    public void LoadNextContext(int index)
    {
        if (currentNode.IsLeafNode) return;

        TreeNode<IDialogueContext> next = currentNode.GetChild(index);
        if (next != null)
        {
            contexts.Enqueue(next.node);
            currentNode = next;
        }
        else
        {
            Debug.Log("Conversation has no more contexts");
            ConversationMgr.Instance.EndConversation();
        }
    }

    private Speaker GetSpeaker(IDialogueContext context)
    {
        if (context.OnlyPlayer())
        {
            return speakers[playerSpeaker];
        }

        if (context.ProceedToNextSpeaker())
        {
            return speakers[++currentSpeakerIndex % speakerCount];
        }

        return speakers[currentSpeakerIndex % speakerCount];
    }

    public void EnqueueContext(IDialogueContext context)
    {
        contexts.Enqueue(context);
    }

    public void ProceedToNextSpeaker()
    {
        currentSpeakerIndex++;
    }
}