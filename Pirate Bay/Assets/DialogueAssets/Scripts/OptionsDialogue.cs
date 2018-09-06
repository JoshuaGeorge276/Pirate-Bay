using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Options Dialogue", menuName = "Dialogue Context/Options Dialogue", order = 1)]
public class OptionsDialogue : IDialogueContext {

    public DialogueTypes.Triggers triggerType;

    public string[] options;

    public DialogueTypes.Type[] responseTriggers;

    private IIterable conversation;

    public override IDialogueDisplayer Display(Speaker speaker, IIterable conversation)
    {
        this.conversation = conversation;

        UnityAction<int>[] onClicks = new UnityAction<int>[options.Length];

        for(int i = 0; i < onClicks.Length; i++)
        {
            onClicks[i] = OptionSelected;
        }

        return new OptionsDialogueDisplayer(options, onClicks, speaker);
    }

    public void OptionSelected(int i)
    {
        Player2NPCConversation convo = (Player2NPCConversation)conversation;
        DialogueOfType dialogueOfType = CreateInstance<DialogueOfType>();
        dialogueOfType.Init(responseTriggers[i]);
        convo.EnqueueContext(dialogueOfType);
        convo.Next();   
        Debug.Log("Add new context with response trigger: " + responseTriggers[i].ToString());
    }
}
