using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "Dialogue Bundle/NPC Dialogue", order = 1)]
[System.Serializable]
public class NPCDialogueBundle : ScriptableObject {

    [System.Serializable]
	public class DialogueBundle
    {
        public DialogueTypes.Type type;
        public string[] dialogue;
    }

    [SerializeField]
    public List<DialogueBundle> dialogueBundles = new List<DialogueBundle>();
        
    private Dictionary<DialogueTypes.Type, ScriptedDialogue> dialogueBundlesDic;
    public void OnEnable()
    {
        //dialogueBundles = new List<DialogueBundle>();

        Init();
    }

    void Init()
    {
        dialogueBundlesDic = new Dictionary<DialogueTypes.Type, ScriptedDialogue>();

        foreach (DialogueBundle bundle in dialogueBundles)
        {
            ScriptedDialogue scripted = CreateInstance<ScriptedDialogue>();
            scripted.Init(bundle.dialogue);
            dialogueBundlesDic.Add(bundle.type, scripted);
        }
    }

    public ScriptedDialogue GetDialogueOfType(DialogueTypes.Type type)
    {
        // If the bundles contains the key then return the dialogue but if it doesnt then return an empty scripted dialogue.
        return (dialogueBundlesDic.ContainsKey(type)) ? dialogueBundlesDic[type] : CreateInstance<ScriptedDialogue>();
    }
}
