using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockFace.DialogueSystem
{
    public enum ConversationState
    {
        NotStarted,
        OnGoing,
        Finished
    }

    public abstract class Conversation
    {
        protected ConversationState state;

        protected ConversationLayout layout;

        protected int numberOfSpeakers;
        protected int currentSpeaker;

        public Conversation(ConversationLayout layout, int numberOfSpeakers)
        {
            this.layout = layout;
            this.numberOfSpeakers = numberOfSpeakers;
            currentSpeaker = 0;
            state = ConversationState.OnGoing;
        }

        public abstract DialogueContext GetNext();
        
    }
}

