using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Core.Audio
{
    [CreateAssetMenu(fileName = "NewAudioClipList", menuName = "AudioClipList")]
    public class AudioClipList : ScriptableObject
    {
        [SerializeField]
        private List<AudioClip> _audioClips = new List<AudioClip>();

        [HideInInspector]
        [SerializeField]
        private List<int> _audioHashList = new List<int>();

        public AudioClip GetClipByName(string a_name)
        {
            int hash = a_name.GetHashCode();

            int count = _audioHashList.Count;
            for (int i = 0; i < count; ++i)
            {
                if (_audioHashList[i] == hash)
                    return _audioClips[i];
            }
            Debug.LogError("AudioClipList: Unable to find Clip by Name");
            return null;
        }

        public void EnumerateList()
        {
            _audioHashList.Clear();

            int count = _audioClips.Count;
            for (int i = 0; i < count; ++i)
            {
                int hash = _audioClips[i].name.GetHashCode();
                
                if(_audioHashList.Contains(hash))
                    Debug.LogWarning("AudioClipList: Duplicate has detected on enumeration, duplicate element present, suggest removing to prevent issues.");

                _audioHashList.Add(hash);
            }
            Debug.Log("AudioClip Hashlist Updated!");
            EditorUtility.SetDirty(this);
        }

    }

    [CustomEditor(typeof(AudioClipList))]
    public class AudioClipListEditor : Editor
    {
        private ReorderableList _reorderableList;
        private AudioClipList _list;

        public void OnEnable()
        {
            _list = (AudioClipList) target;

            _reorderableList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("_audioClips"),
                true, true, true, true);

            _reorderableList.drawElementCallback = DrawElement;
            _reorderableList.drawHeaderCallback = DrawHeader;
        }

        private void DrawHeader(Rect a_rect)
        {
            EditorGUI.LabelField(a_rect, "Audio Clips");
        }

        private void DrawElement(Rect a_rect, int a_index, bool a_isActive, bool a_isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(a_index);
            a_rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(
                    a_rect.x,
                    a_rect.y,
                    EditorGUIUtility.currentViewWidth - 50,
                    EditorGUIUtility.singleLineHeight),
                element,
                GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("UpdateListHashes"))
            {
                _list.EnumerateList();
                serializedObject.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

    }
}

