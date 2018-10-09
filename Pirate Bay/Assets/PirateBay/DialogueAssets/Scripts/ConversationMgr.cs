using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationMgr : MonoBehaviour {

    public static ConversationMgr Instance;

    public RespondSpeaker playerSpeaker;
    public RespondSpeaker npcSpeaker;

    public ConversationLayout layout;

    private TreeList<IDialogueContext> dialogueTree;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialogueTree = layout.GetDialogueTree();
    }

    public void StartScriptedConversation()
    {
        Conversation convo = new Conversation(dialogueTree, new Speaker[] { playerSpeaker, npcSpeaker });
        convo.StartConversation();
    }

    public void EndConversation()
    {
        DialogueCanvasMgr.Instance.Hide();
    }
}
