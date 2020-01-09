using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAC.General;

namespace Memorama.UI
{
    public class GameModeSelectionMenu : MemoramaMenu
    {
        public void SetGameMode(int mode)
        {
            if (mode < 0 || mode > (int)GameModes.THREE_SUITS || mode > toggle_array.Length) return;

            MemoramaManager.Instance.SetGameMode(mode);

            DisableAll();
            if (mode == 0)
            {
                continue_button.SemiDisable();
                continue_button.SetInteraction(false);
            }
            else
            {
                toggle_array[mode - 1].SemiEnable();
                continue_button.SemiEnable();
                continue_button.SetInteraction(true);
            }
        }

        public void Restart()
        {
            DisableAll();
            continue_button.SemiDisable();
            continue_button.SetInteraction(false);
        }

        private void DisableAll()
        {
            foreach (Toggle toggle in toggle_array)
                toggle.SemiDisable();
        }

        #region OVERRIDEN METHODS
        public override void CreateToggleArray()
        {
            if (toggle_array == null || toggle_array.Length != (int)GameModes.THREE_SUITS)
                toggle_array = new Toggle[(int)GameModes.THREE_SUITS];
        }

        protected override void OnStart()
        {
            foreach (Toggle toggle in toggle_array)
                toggle.SemiDisable();
            continue_button.SemiDisable();
            continue_button.SetInteraction(false);

            gameObject.SetActive(false);
        }
        #endregion
    }
}