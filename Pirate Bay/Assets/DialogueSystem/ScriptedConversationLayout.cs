using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockFace.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Scripted Conversation Layout", menuName = "DialogueSystem/ConversationLayouts/ScriptedConversationLayout")]
    public class ScriptedConversationLayout : ConversationLayout
    {
        public List<GenericDialogue> dialogue;

        public ScriptedConversationLayout()
        {
            dialogue = new List<GenericDialogue>();
        }

        public DialogueContext GetContext(int index)
        {
            return dialogue[index];
        }

        public int GetCount()
        {
            return dialogue.Count;
        }
    }
}

