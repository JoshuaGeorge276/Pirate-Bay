using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(GamingEnvironment))]
public class GamingEnvironmentEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
            serializedObject.FindProperty("_environmentObjects"),
            true, true, true , true);

        list.drawHeaderCallback = DrawHeaderCallback;
        list.drawElementCallback = DrawElementCallback;
    }

    private void DrawHeaderCallback(Rect a_rect)
    {
        EditorGUI.LabelField(a_rect, "Environment Objects");
    }

    private void DrawElementCallback(Rect a_rect, int a_index, bool a_isActive, bool a_isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(a_index);

        a_rect.y += 2;

        EditorGUI.PropertyField(new Rect(a_rect.x, a_rect.y, EditorGUIUtility.currentViewWidth - 50, EditorGUIUtility.singleLineHeight),
            element, GUIContent.none);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

#endif

