using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAC.General;

namespace Memorama.UI
{
    public class SuitSelectionMenu : MemoramaMenu
    {
        #region PUBLIC METHODS
        public void AddSuit(int suit)
        {
            if (hasSelectedRequiredSuits()) return;
            if (suit < 0 || suit >= (int)CardsSuit.counter || suit >= toggle_array.Length) return;

            MemoramaManager.Instance.AddMatchSuit(suit);

            if (hasSelectedRequiredSuits())
            {
                DisableRemainingToggle();
                continue_button.SetInteraction(true);
                continue_button.SemiEnable();
            }
            else
            {
                continue_button.SemiDisable();
                continue_button.SetInteraction(false);
            }
        }

        public void RemoveSuit(int suit)
        {
            if (!GV.GameSuits.Contains((CardsSuit)suit)) return;
            MemoramaManager.Instance.AddMatchSuit(suit);

            if (hasSelectedRequiredSuits())
            {
                continue_button.SemiEnable();
                continue_button.SetInteraction(true);
                DisableRemainingToggle();
            }
            else
            {
                SetAllToggle(true);
                continue_button.SemiDisable();
                continue_button.SetInteraction(false);
            }
        }

        public void Restart()
        {
            foreach (Toggle toggle in toggle_array) {
                toggle.SemiDisable();
                toggle.SetInteraction(true);
            }

            continue_button.SemiDisable();
            continue_button.SetInteraction(false);
        }
        #endregion

        #region PRIVATE METHODS
        private bool hasSelectedRequiredSuits()
        {
            return GV.GameSuits.Count >= (int)GV.GameMode;
        }

        private void DisableRemainingToggle()
        {
            SetAllToggle(false);
            foreach (int suit in GV.GameSuits)
                toggle_array[suit].SetInteraction(true);
        }

        private void SetAllToggle(bool active)
        {
            foreach (Toggle toggle in toggle_array)
                toggle.SetInteraction(active);
        }
        #endregion

        #region OVERRIDEN METHODS
        public override void CreateToggleArray()
        {
            if (toggle_array == null || toggle_array.Length != (int)CardsSuit.counter)
                toggle_array = new Toggle[(int)CardsSuit.counter];
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