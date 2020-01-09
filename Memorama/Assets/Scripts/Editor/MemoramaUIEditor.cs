using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Memorama.CustomEditors
{
    [CustomEditor(typeof(MemoramaUI))]
    public class MemoramaUIEditor : Editor
    {
        #region EDITOR METHODS
        private void OnEnable()
        {
            SetVariables();
            ((MemoramaUI)target).CreatePanels();
            ((MemoramaUI)target).CreatePlayerUI();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfDirtyOrScript();
            GUILayout.Space(6);
            DrawPanels();
            GUILayout.Space(6);
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(6);
            DrawInGameMenuVariables();
            GUILayout.Space(6);
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region VARIABLES
        private int max_width = 350;

        private SerializedProperty main_panels;
        private SerializedProperty panel_names;
        private SerializedProperty In_Game;

        private SerializedProperty player_info;
        #endregion

        #region PRIVATE METHODS
        private void DrawPanels()
        {
            for (int panel = 0; panel < main_panels.arraySize; ++panel)
            {
                DrawPanel(main_panels.GetArrayElementAtIndex(panel), panel_names.GetArrayElementAtIndex(panel).stringValue);
            }
        }

        private void DrawInGameMenuVariables()
        {
            EditorGUILayout.LabelField("In-Game UI Elements");
            GUILayout.Space(6);
            EditorGUI.indentLevel++;
            for(int i = 0;i< player_info.arraySize;++i)
            EditorGUILayout.PropertyField(player_info.GetArrayElementAtIndex(i), new GUIContent(((UIPlayerInfo)i).ToString()), GUILayout.MaxWidth(max_width));
            EditorGUI.indentLevel--;
        }

        private void SetVariables()
        {
            main_panels = serializedObject.FindProperty("main_panels");
            panel_names = serializedObject.FindProperty("panel_names");
            In_Game = serializedObject.FindProperty("In_Game");

            player_info = In_Game.FindPropertyRelative("player_info");
        }

        private void DrawPanel(SerializedProperty panel, string panel_name)
        {
            EditorGUILayout.PropertyField(panel, new GUIContent(panel_name), GUILayout.MaxWidth(max_width));
        }
        #endregion
    }
}