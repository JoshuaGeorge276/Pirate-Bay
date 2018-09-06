using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation Layout", menuName = "Conversation Layouts/Scripted Converation Layout")]
public class ScriptedConversationLayout : ScriptableObject {

    public enum SpeakerNumber
    {
        One = 0,
        Two,
        Three,
        Four,
        Five
    }

    [System.Serializable]
    public struct Dialogue
    {
        [Tooltip("Title for this section of dialogue")]
        public string name;
        public SpeakerNumber speaker;
        public ScriptedDialogue dialogue;   
    }

    [Range(1, 5)]
    [SerializeField]
    private int speakerCount = 1;

    public Dialogue[] conversation;

    public int SpeakerCount { get { return speakerCount; } }

    public int DialogueCount { get { return conversation.Length; } }

    public ScriptedDialogue GetNextDialogue(int index)
    {
        return conversation[index].dialogue;
    }

    public int GetNextSpeaker(int index)
    {
        return (int)conversation[index].speaker;
    }

}
