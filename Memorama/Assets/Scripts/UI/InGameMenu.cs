using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Memorama.UI
{
    [System.Serializable]
    public class InGameMenu
    {
        [SerializeField]
        private Text[] player_info;

        public void UpdatePlayerUI(UIPlayerInfo info, string text) {
            if (player_info == null || player_info.Length < (int)UIPlayerInfo.counter) return;
            if (player_info[(int)info] == null) return;
            player_info[(int)info].text = text;
        }

        internal void VerifyPlayerUI() 
        {
            if (player_info == null || player_info.Length < (int)UIPlayerInfo.counter)
                player_info = new Text[(int)UIPlayerInfo.counter];
        }
    }
}
