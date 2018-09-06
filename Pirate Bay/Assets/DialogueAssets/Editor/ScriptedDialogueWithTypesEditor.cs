using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptedDialogueWithTypes))]
public class ScriptedDialogueWithTypesEditor : Editor {

    private void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ScriptedDialogueWithTypes myScript = (ScriptedDialogueWithTypes)target;
        if (GUILayout.Button("Add New Scripted Dialogue"))
        {
            myScript.AddScriptedDialogue();
        }

        if (GUILayout.Button("Add New Options Dialogue"))
        {
            myScript.AddOptionsDialogue();
        }
    }

}
