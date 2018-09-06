using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationMgr : MonoBehaviour {

    public static ConversationMgr Instance;

    public PlayerSpeaker playerSpeaker;
    public NPCSpeaker npcSpeaker;

    public CustomConversationLayout layout;

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
    }

    public void StartScriptedConversation()
    {
        Player2NPCConversation convo = new Player2NPCConversation(layout, playerSpeaker, npcSpeaker);
        // TODO: Create conversation object.
    }

    public void EndConversation()
    {
        DialogueCanvasMgr.Instance.Hide();
    }
}
