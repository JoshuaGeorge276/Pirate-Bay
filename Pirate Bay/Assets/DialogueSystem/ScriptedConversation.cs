using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockFace.DialogueSystem
{
    public class ScriptedConversation : Conversation
    {
        private BasicSpeaker primarySpeaker;
        private BasicSpeaker secondarySpeaker;
        private ScriptedConversationLayout scriptedLayout;
        private int conversationIndex;
        private int maxConversationIndex;

        public ScriptedConversation(ScriptedConversationLayout layout, BasicSpeaker primarySpeaker, BasicSpeaker secondarySpeaker) : base(layout, 2)
        {
            this.primarySpeaker = primarySpeaker;
            this.secondarySpeaker = secondarySpeaker;
            scriptedLayout = layout;
            conversationIndex = 0;
            maxConversationIndex = layout.GetCount();
        }

        public void UpdateState()
        {
            if (conversationIndex >= maxConversationIndex - 1)
                state = ConversationState.Finished;
        }

        public bool Finished()
        {
            return state == ConversationState.Finished;
        }

        public override DialogueContext GetNext()
        {
            UpdateState();

            DialogueContext context = scriptedLayout.GetContext(conversationIndex);

            switch (currentSpeaker % 2)
            {
                case 0:
                    {
                        SpeakerState state = primarySpeaker.Speak(context);
                        break;
                    }
                    
                case 1:
                    {
                        SpeakerState state = secondarySpeaker.Speak(context);
                        break;
                    }
                    
            }

            currentSpeaker++;
            conversationIndex++;

            return context;
        }

        public BasicSpeaker GetCurrentSpeaker()
        {
            switch(currentSpeaker % 2)
            {
                case 0:
                    return primarySpeaker;
                case 1:
                    return secondarySpeaker;
            }

            Debug.LogWarning("Could not get speaker with current speaker index being " + (currentSpeaker % 2));
            return null;
        }
    }
}

