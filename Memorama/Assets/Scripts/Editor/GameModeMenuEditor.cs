using Memorama.UI;
using UnityEditor;
using UnityEngine;

namespace Memorama.Editors
{
    [CustomEditor(typeof(GameModeSelectionMenu))]
    public class GameModeMenuEditor : MemoramaMenuEditor<GameModes>
    {
        #region OVERRIDEN METHODS
        protected override void OnEnableActions()
        {
            ((GameModeSelectionMenu)target).CreateToggleArray();
        }

        protected override void OnInspectorDrawActcions()
        {
            GUILayout.Space(6);
            DrawScriptField((GameModeSelectionMenu)target, typeof(GameModeSelectionMenu));
            GUILayout.Space(8);
            DrawContinueButton();
            GUILayout.Space(8);
            DrawToggleArray<GameModes>(1, "The toggle button representing the Game Mode button.");
            GUILayout.Space(6);
        }
        #endregion
    }
}
