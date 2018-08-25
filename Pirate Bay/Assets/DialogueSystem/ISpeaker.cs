using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockFace.DialogueSystem
{
    public enum SpeakerState
    {
        Fine,
        Leave,
        Finished
    }

    public interface ISpeaker
    {
        SpeakerState Speak(DialogueContext context);

    }
}

