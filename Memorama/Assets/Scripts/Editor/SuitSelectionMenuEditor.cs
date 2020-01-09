using Memorama.UI;
using UnityEditor;
using UnityEngine;

namespace Memorama.Editors
{
    [CustomEditor(typeof(SuitSelectionMenu))]
    public class SuitSelectionMenuEditor : MemoramaMenuEditor<CardsSuit>
    {
        #region OVERRIDEN METHODS
        protected override void OnEnableActions()
        {
            ((SuitSelectionMenu)target).CreateToggleArray();
        }

        protected override void OnInspectorDrawActcions()
        {
            GUILayout.Space(6);
            DrawScriptField((SuitSelectionMenu)target, typeof(SuitSelectionMenu));
            GUILayout.Space(8);
            DrawContinueButton();
            GUILayout.Space(8);
            DrawToggleArray<CardsSuit>(0, "The toggle button representing the Suit button.");
            GUILayout.Space(6);
        }
        #endregion
    }
}
