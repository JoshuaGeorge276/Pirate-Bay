using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Keeping this around for reference
[CreateAssetMenu(fileName = "New Dialogue with Types", menuName = "Scripted Dialogue With Types", order = 1)]
public class ScriptedDialogueWithTypes : ScriptableObject {

    [System.Serializable]
    public class DialogueOfTypeClass
    {
        public DialogueTypes.Type type;
        [SerializeField]
        public IDialogueContext dialogue;
    }

    public List<DialogueOfTypeClass> dialogueGroupWithTypes;

    public List<IDialogueContext> dialogueContext;

    public void OnEnable()
    {
        if(dialogueGroupWithTypes == null)
        {
            dialogueGroupWithTypes = new List<DialogueOfTypeClass>();
        }

        dialogueContext = new List<IDialogueContext>();
    }

    public void AddScriptedDialogue()
    {
        DialogueOfTypeClass newDialogueOfType = new DialogueOfTypeClass();
        newDialogueOfType.dialogue = CreateInstance<ScriptedDialogue>();
        dialogueContext.Add(CreateInstance<ScriptedDialogue>());
    }

    public void AddOptionsDialogue()
    {
        DialogueOfTypeClass newDialogueOfType = new DialogueOfTypeClass();
        newDialogueOfType.dialogue = CreateInstance<OptionsDialogue>();
        dialogueContext.Add(CreateInstance<OptionsDialogue>());
    }

}
