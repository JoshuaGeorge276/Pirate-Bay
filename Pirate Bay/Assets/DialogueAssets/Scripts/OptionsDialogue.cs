using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(fileName = "New Options Dialogue", menuName = "Dialogue Context/Options Dialogue", order = 1)]
public class OptionsDialogue : IDialogueContext {

    public DialogueOption[] options = new DialogueOption[4];

    private Conversation conversation;
    public override void Speak(Conversation conversation, Speaker speaker)
    {
        this.conversation = conversation;
        Display(conversation, speaker);
    }

    public void OptionSelected(int i)
    {
        Player2NPCConversation convo = (Player2NPCConversation)conversation;
        DialogueOption selected = options[i];
        if (!selected.endConversation)
        {
            convo.EnqueueContext(selected.nextContext);
            if (selected.proceedToNextSpeaker)
            {
                conversation.ProceedToNextSpeaker();
            }
        }
        convo.Next();   
    }

    public override IDialogueDisplayer Display(IIterable conversation, Speaker speaker)
    {
        string[] optionsText = new string[4];
        UnityAction<int>[] onClicks = new UnityAction<int>[options.Length];

        for (int i = 0; i < 4; i++)
        {
            optionsText[i] = options[i].dialogue;
            onClicks[i] = OptionSelected;
        }

        return new OptionsDialogueDisplayer(optionsText, onClicks, speaker);
    }
}

[System.Serializable]
public class DialogueOption
{
    public string dialogue;
    public bool endConversation;
    public bool proceedToNextSpeaker;

    [SerializeField]
    public IDialogueContext nextContext;

}

[CustomEditor(typeof(OptionsDialogue))]
public class OptionsDialogueEditor : Editor
{
    static int optionToModify;

    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginVertical("box");

        OptionsDialogue optionsDialogue = target as OptionsDialogue;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Options Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        optionToModify = EditorGUILayout.IntSlider("Option to modify", optionToModify, 1, 4);

        if (GUILayout.Button("Add Scripted Dialogue"))
        {
            ScriptedDialogue newDialogue = CreateInstance<ScriptedDialogue>();
            optionsDialogue.options[optionToModify - 1].nextContext = newDialogue;

            AssetDatabase.AddObjectToAsset(newDialogue, "Player");
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Add Options Dialogue"))
        {
            optionsDialogue.options[optionToModify - 1].nextContext = CreateInstance<OptionsDialogue>();
            EditorUtility.SetDirty(optionsDialogue.options[optionToModify - 1].nextContext);
        }

        if (GUILayout.Button("Add Typed Dialogue"))
        {
            optionsDialogue.options[optionToModify - 1].nextContext = CreateInstance<DialogueOfType>();
            EditorUtility.SetDirty(optionsDialogue.options[optionToModify - 1].nextContext);
        }

        if (GUILayout.Button("Remove Context"))
        {
            if(optionsDialogue.options[optionToModify - 1].nextContext != null)
            {
                DestroyImmediate(optionsDialogue.options[optionToModify - 1].nextContext);
                optionsDialogue.options[optionToModify - 1].nextContext = null;
                EditorUtility.SetDirty(optionsDialogue.options[optionToModify - 1].nextContext);
            }
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        Repaint();

    }

}
