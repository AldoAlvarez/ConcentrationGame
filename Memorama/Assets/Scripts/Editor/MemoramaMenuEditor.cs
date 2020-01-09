using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorama.UI;
using UnityEditor;

namespace Memorama.Editors
{
    [CustomEditor(typeof(MemoramaMenu))]
    public class MemoramaMenuEditor<T>  : Editor where T : struct, System.IConvertible
    {
        #region EDITOR METHODS
        private void OnEnable()
        {
            SetVariables();
            OnEnableActions();
        }
        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = 120;
            serializedObject.UpdateIfDirtyOrScript();
            OnInspectorDrawActcions();    
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region VARIABLES
        protected SerializedProperty toggle_array;
        protected SerializedProperty continue_button;
        private const int max_width = 250;
        #endregion

        #region PRIVATE METHODS
        private void SetVariables() {
            toggle_array = serializedObject.FindProperty("toggle_array");
            continue_button = serializedObject.FindProperty("continue_button");
        }
        #endregion

        #region PROTECTED METHODS
        protected void DrawToggleArray<T>(int label_offset, string label_tooltip = "") where T: struct, System.IConvertible {
            string label = string.Empty;
            for (int i = 0; i < toggle_array.arraySize; ++i)
            {
                label = (System.Enum.Parse(typeof(T), (i + label_offset).ToString(), true)).ToString();
                EditorGUILayout.PropertyField(toggle_array.GetArrayElementAtIndex(i), new GUIContent(label, label_tooltip), GUILayout.MaxWidth(max_width));
            }
        }
        protected void DrawContinueButton() {
            EditorGUILayout.PropertyField(continue_button, new GUIContent("Continue Bttn.", "The reference to the toggle button that will make possible to procceed to the next menu."), GUILayout.MaxWidth(max_width));
        }

        protected void DrawScriptField(MonoBehaviour script_type, System.Type object_type)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script: ", MonoScript.FromMonoBehaviour(script_type), object_type, false);
            EditorGUI.EndDisabledGroup();
        }

        protected virtual void OnEnableActions() { }
        protected virtual void OnInspectorDrawActcions() { }
        #endregion
    }
}