using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockFace.DialogueSystem
{
    public class BasicSpeaker : MonoBehaviour, ISpeaker
    {
        public string name = "";
        public Sprite image; 

        public SpeakerState Speak(DialogueContext context)
        {
            return SpeakerState.Fine;
        }
    }
}

